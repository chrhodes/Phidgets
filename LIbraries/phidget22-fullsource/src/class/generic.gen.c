/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

#include "device/genericdevice.h"
static void CCONV PhidgetGeneric_errorHandler(PhidgetChannelHandle ch, Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetGeneric_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetGeneric_setStatus(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetGeneric_getStatus(PhidgetChannelHandle phid, BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetGeneric_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetGeneric_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetGeneric_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetGeneric {
	struct _PhidgetChannel phid;
	uint32_t INPacketLength;
	uint32_t OUTPacketLength;
	PhidgetGeneric_OnPacketCallback Packet;
	void *PacketCtx;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetGenericHandle ch;
	int version;

	ch = (PhidgetGenericHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 0) {
		logerr("%"PRIphid": bad version %d != 0", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	ch->INPacketLength = getBridgePacketUInt32ByName(bp, "INPacketLength");
	ch->OUTPacketLength = getBridgePacketUInt32ByName(bp, "OUTPacketLength");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetGenericHandle ch;

	ch = (PhidgetGenericHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",INPacketLength=%u"
	  ",OUTPacketLength=%u"
	  ,0 /* class version */
	  ,ch->INPacketLength
	  ,ch->OUTPacketLength
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetReturnCode res;

	res = EPHIDGET_OK;

	switch (bp->vpkt) {
	case BP_SENDPACKET:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetGenericDeviceHandle parentGeneric;
	PhidgetGenericHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetGenericHandle)phid;

	ret = EPHIDGET_OK;

	parentGeneric = (PhidgetGenericDeviceHandle)phid->parent;

	switch (phid->UCD->uid) {
	case PHIDCHUID_USB_GENERIC:
		ch->INPacketLength = parentGeneric->INPacketLength[ch->phid.index];
		ch->OUTPacketLength = parentGeneric->OUTPacketLength[ch->phid.index];
		break;
	case PHIDCHUID_VINT_GENERIC:
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}


	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_USB_GENERIC:
		break;
	case PHIDCHUID_VINT_GENERIC:
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	return (ret);
}

static void CCONV
_fireInitialEvents(PhidgetChannelHandle phid) {

}

static void CCONV
PhidgetGeneric_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetGeneric));
}

API_PRETURN
PhidgetGeneric_create(PhidgetGenericHandle *phidp) {

	CHANNELCREATE_BODY(Generic, PHIDCHCLASS_GENERIC);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetGeneric_delete(PhidgetGenericHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetGeneric_sendPacket(PhidgetGenericHandle ch, const uint8_t *packet, size_t packetLen) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_GENERIC);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SENDPACKET, NULL, NULL, "%*R", packetLen,
	  packet);
}

API_VRETURN
PhidgetGeneric_sendPacket_async(PhidgetGenericHandle ch, const uint8_t *packet, size_t packetLen,
  Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_GENERIC) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SENDPACKET, fptr, ctx, "%*R", packetLen,
	  packet);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetGeneric_getINPacketLength(PhidgetGenericHandle ch, uint32_t *INPacketLength) {

	TESTPTRL(ch);
	TESTPTRL(INPacketLength);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_GENERIC);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_VINT_GENERIC:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*INPacketLength = ch->INPacketLength;
	if (ch->INPacketLength == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetGeneric_getOUTPacketLength(PhidgetGenericHandle ch, uint32_t *OUTPacketLength) {

	TESTPTRL(ch);
	TESTPTRL(OUTPacketLength);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_GENERIC);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_VINT_GENERIC:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*OUTPacketLength = ch->OUTPacketLength;
	if (ch->OUTPacketLength == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetGeneric_setOnPacketHandler(PhidgetGenericHandle ch, PhidgetGeneric_OnPacketCallback fptr,
  void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_GENERIC);

	ch->Packet = fptr;
	ch->PacketCtx = ctx;

	return (EPHIDGET_OK);
}
