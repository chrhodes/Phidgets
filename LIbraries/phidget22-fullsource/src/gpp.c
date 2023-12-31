/*
 * This file is part of libphidget22
 *
 * Copyright 2015 Phidgets Inc <patrick@phidgets.com>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, see
 * <http://www.gnu.org/licenses/>
 */

#include "phidgetbase.h"
#include "gpp.h"

PhidgetReturnCode
PhidgetGPP_dataInput(PhidgetDeviceHandle device, unsigned char *buffer, size_t length) {
	PhidgetReturnCode result = EPHIDGET_OK;

	//if response bits are set (0x00 is ignore), then store response
	if (buffer[0] & 0x3f)
		device->GPPResponse = buffer[0];

	return result;
}

static PhidgetReturnCode
GPP_getResponse(PhidgetDeviceHandle device, int packetType, int timeout) {
	while ((device->GPPResponse & 0x3f) != packetType && timeout > 0) {
		mos_usleep(20000);
		timeout -= 20;
	}

	//Didn't get the response!
	if ((device->GPPResponse & 0x3f) != packetType) {
		return (EPHIDGET_TIMEOUT);
	}

	if (device->GPPResponse & PHID_GENERAL_PACKET_FAIL)
		return (EPHIDGET_UNEXPECTED);

	return (EPHIDGET_OK);
}


static PhidgetReturnCode
GPP_setConfigTable(PhidgetDeviceHandle device, const unsigned char *data, size_t length, int index, int packetType) {
	PhidgetReturnCode res;
	unsigned char buffer[MAX_OUT_PACKET_SIZE] = { 0 };
	int i, j;
	size_t buflen;

	assert(device);
	if (!ISATTACHED(device))
		return (EPHIDGET_NOTATTACHED);

	buflen = getMaxOutPacketSize(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | packetType;
	buffer[1] = index;
	for (i = 2, j = 0; i < (int)buflen && j < (int)length; i++, j++)
		buffer[i] = data[j];

	if ((res = PhidgetDevice_sendpacket(device, buffer, i)) != EPHIDGET_OK)
		return (res);

	while (j < (int)length && res == EPHIDGET_OK) {
		memset(buffer, 0, sizeof(buffer));
		buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_CONTINUATION;
		for (i = 1; i < (int)buflen && j < (int)length; i++, j++)
			buffer[i] = data[j];
		if ((res = PhidgetDevice_sendpacket(device, buffer, i)) != EPHIDGET_OK)
			return (res);
	}

	return (EPHIDGET_OK);
}

/**
 * General Packet Protocol
 *
 * These are devices on the new M3. They all support this protocol.
 *  MSB is set is buf[0] for incoming and outgoing packets.
 */
BOOL
deviceSupportsGeneralPacketProtocol(PhidgetDeviceHandle device) {
	switch (device->deviceInfo.UDD->uid) {
	case PHIDUID_1041:
	case PHIDUID_1043:
	case PHIDUID_1042:
	case PHIDUID_1044:
	case PHIDUID_1032:
	case PHIDUID_1024:
	case PHIDUID_1067:
	case PHIDUID_HUB0000:
	case PHIDUID_HUB0002:
	case PHIDUID_HUB0004:
	case PHIDUID_HUB0005:
		return PTRUE;

	case PHIDUID_FIRMWARE_UPGRADE_M3_USB:
	case PHIDUID_FIRMWARE_UPGRADE_M3_SPI:
		return PTRUE;

	case PHIDUID_GENERIC:
		return PTRUE;

	default:
		return PFALSE;
	}
}

PhidgetReturnCode
GPP_upgradeFirmware(PhidgetDeviceHandle device, const unsigned char *data, size_t length, PhidgetChannelHandle channel) {
	int i, j, index, indexEnd;
	PhidgetReturnCode res;
	unsigned char buffer[MAX_OUT_PACKET_SIZE] = { 0 };
	size_t buflen;
	double progress, lastProgress;

	TESTPTR(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buflen = getMaxOutPacketSize(device);

	device->GPPResponse = 0;

	index = ((length & 0xf000) >> 12) + 1;
	indexEnd = length & 0xfff;
	if (channel) {
		bridgeSendToChannel(channel, BP_PROGRESSCHANGE, "%g", 0.0);
		lastProgress = 0;
	}
	j = 0;

	while (index) {
		int secLength = ((int)length) - ((index - 1) * 0x1000);
		if (secLength > 0x1000)
			secLength = 0x1000;

		buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_FIRMWARE_UPGRADE_WRITE_SECTOR;
		buffer[1] = index;
		buffer[2] = secLength;
		buffer[3] = secLength >> 8;

		for (i = 4; i < (int)buflen && j < indexEnd; i++, j++)
			buffer[i] = data[j];

		if ((res = PhidgetDevice_sendpacket(device, buffer, i)) != EPHIDGET_OK)
			goto done;

		while (j < indexEnd && res == EPHIDGET_OK) {
			memset(buffer, 0, sizeof(buffer));
			buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_CONTINUATION;
			for (i = 1; i < (int)buflen && j < indexEnd; i++, j++)
				buffer[i] = data[j];

			if (channel) {
				progress = (double)j / (double)length;
				if (progress - lastProgress >= 0.01) {
					bridgeSendToChannel(channel, BP_PROGRESSCHANGE, "%g", progress);
					lastProgress = progress;
				}
			}

			if ((res = PhidgetDevice_sendpacket(device, buffer, i)) != EPHIDGET_OK)
				goto done;
		}
		index--;
		indexEnd += 0x1000;
	}

done:

	res = GPP_getResponse(device, PHID_GENERAL_PACKET_FIRMWARE_UPGRADE_WRITE_SECTOR, 200);

	if (channel) {
		if (lastProgress != 1)
			bridgeSendToChannel(channel, BP_PROGRESSCHANGE, "%g", 1.0);
	}

	return (res);
}

PhidgetReturnCode
GPP_eraseFirmware(PhidgetDeviceHandle device) {
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_FIRMWARE_UPGRADE_ERASE;

	device->GPPResponse = 0;
	if ((res = PhidgetDevice_sendpacket(device, buffer, 1)) == EPHIDGET_OK)
		res = GPP_getResponse(device, PHID_GENERAL_PACKET_FIRMWARE_UPGRADE_ERASE, 200);

	return (res);
}

PhidgetReturnCode
GPP_eraseConfig(PhidgetDeviceHandle device) {
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_ERASE_CONFIG;

	device->GPPResponse = 0;
	if ((res = PhidgetDevice_sendpacket(device, buffer, 1)) == EPHIDGET_OK)
		res = GPP_getResponse(device, PHID_GENERAL_PACKET_ERASE_CONFIG, 200);

	return (res);
}

PhidgetReturnCode
GPP_reboot_firmwareUpgrade(PhidgetDeviceHandle device) {
	PhidgetUSBConnectionHandle usbconn;
#ifdef SPI_SUPPORT
	PhidgetSPIConnectionHandle spiconn;
#endif
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	//Stop the read/write threads first
	switch (device->connType) {
#ifdef SPI_SUPPORT
	case PHIDCONN_SPI:
		spiconn = PhidgetSPIConnectionCast(device->conn);
		assert(spiconn);
		joinSPIReadThread(spiconn);
		break;
#endif
	case PHIDCONN_USB:
		usbconn = PhidgetUSBConnectionCast(device->conn);
		assert(usbconn);
		joinUSBReadThread(usbconn);
		break;
	}

	//Then send the command
	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_REBOOT_FIRMWARE_UPGRADE;
	res = PhidgetDevice_sendpacket(device, buffer, 1);

#ifdef SPI_SUPPORT
	if (device->connType == PHIDCONN_SPI) {
		clearAttachedSPIDevices();
		mos_usleep(500000);
		populateAttachedSPIDevices();

		if (res == EPHIDGET_TIMEOUT)
			return (EPHIDGET_OK); //expected for spi
	}
#endif

	return (res);
}

PhidgetReturnCode
GPP_reboot_ISP(PhidgetDeviceHandle device) {
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_REBOOT_ISP;
	res = PhidgetDevice_sendpacket(device, buffer, 1);

	return (res);
}

PhidgetReturnCode
GPP_writeFlash(PhidgetDeviceHandle device) {
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_WRITE_FLASH;
	res = PhidgetDevice_sendpacket(device, buffer, 1);

	return (res);
}

#ifdef INCLUDE_UNRELEASED
static PhidgetReturnCode
GPP_zeroConfig(PhidgetDeviceHandle device) {
	PhidgetReturnCode res;
	unsigned char buffer[1];

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	buffer[0] = PHID_GENERAL_PACKET_FLAG | PHID_GENERAL_PACKET_ZERO_CONFIG;
	res = PhidgetDevice_sendpacket(device, buffer, 1);

	return (res);
}
#endif

PhidgetReturnCode
GPP_setLabel(PhidgetDeviceHandle device, const char *label) {
	unsigned char buffer[26] = { 0 };
	PhidgetReturnCode result;

	assert(device);
	TESTATTACHED(device);

	if (!deviceSupportsGeneralPacketProtocol(device))
		return (EPHIDGET_UNSUPPORTED);

	//Label Table Header is: 0x0010001A
	buffer[3] = 0x00; //header high byte
	buffer[2] = 0x10;
	buffer[1] = 0x00;
	buffer[0] = 0x1a; //header low byte

	memcpy(buffer + 4, label, label[0]);

	//Label Table index is: 0
	if ((result=GPP_setDeviceWideConfigTable(device, buffer, 26, 0)) == EPHIDGET_OK)
		return GPP_writeFlash(device);
	return result;
}

PhidgetReturnCode
GPP_setDeviceSpecificConfigTable(PhidgetDeviceHandle device, const unsigned char *data, size_t length, int index) {
	assert(device);
	return GPP_setConfigTable(device, data, length, index, PHID_GENERAL_PACKET_SET_DS_TABLE);
}

PhidgetReturnCode
GPP_setDeviceWideConfigTable(PhidgetDeviceHandle device, const unsigned char *data, size_t length, int index) {
	assert(device);
	return GPP_setConfigTable(device, data, length, index, PHID_GENERAL_PACKET_SET_DW_TABLE);
}

PhidgetReturnCode
PhidgetGPP_upgradeFirmware(PhidgetChannelHandle channel, const unsigned char *data, size_t length) {
	assert(channel);
	assert(channel->parent);
	return GPP_upgradeFirmware(channel->parent, data, length, channel);
}

PhidgetReturnCode
PhidgetGPP_eraseFirmware(PhidgetChannelHandle channel) {
	assert(channel);
	assert(channel->parent);
	return GPP_eraseFirmware(channel->parent);
}

PhidgetReturnCode
PhidgetGPP_reboot_firmwareUpgrade(PhidgetChannelHandle channel) {
	assert(channel);
	assert(channel->parent);
	return GPP_reboot_firmwareUpgrade(channel->parent);
}

PhidgetReturnCode
PhidgetGPP_setLabel(PhidgetChannelHandle channel, const char *label) {
	assert(channel);
	assert(channel->parent);
	return GPP_setLabel(channel->parent, label);
}

/****************
 * Exported API *
 ****************/


#ifdef INCLUDE_UNRELEASED
API_PRETURN
Phidget_eraseConfig(PhidgetHandle phid) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_eraseConfig(phid->parent);
}

API_PRETURN
Phidget_rebootISP(PhidgetHandle phid) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_reboot_ISP(phid->parent);
}

API_PRETURN
Phidget_zeroConfig(PhidgetHandle phid) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_zeroConfig(phid->parent);
}

API_PRETURN
Phidget_setDeviceSpecificConfigTable(PhidgetHandle phid, int index, const uint8_t *data, size_t dataLen) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_setConfigTable(phid->parent, data, dataLen, index, PHID_GENERAL_PACKET_SET_DS_TABLE);
}

API_PRETURN
Phidget_setDeviceWideConfigTable(PhidgetHandle phid, int index, const uint8_t *data, size_t dataLen) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_setConfigTable(phid->parent, data, dataLen, index, PHID_GENERAL_PACKET_SET_DW_TABLE);
}
#endif

API_PRETURN
Phidget_writeFlash(PhidgetHandle phid) {
	TESTPTR(phid);
	TESTPTR(phid->parent);
	TESTCHANNEL(phid);
	return GPP_writeFlash(phid->parent);
}
