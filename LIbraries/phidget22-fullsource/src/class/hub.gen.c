/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

#include "device/hubdevice.h"
static void CCONV PhidgetHub_errorHandler(PhidgetChannelHandle ch, Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetHub_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetHub_setStatus(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetHub_getStatus(PhidgetChannelHandle phid, BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetHub_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetHub_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetHub_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetHub {
	struct _PhidgetChannel phid;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	int version;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 0) {
		logerr("%"PRIphid": bad version %d != 0", phid, version);
		return (EPHIDGET_BADVERSION);
	}


	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ,0 /* class version */
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetReturnCode res;

	res = EPHIDGET_OK;

	switch (bp->vpkt) {
	case BP_SETCALIBRATIONVALUES:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_SETFIRMWAREUPGRADEFLAG:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_SETPORTMODE:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_SETPORTPOWER:
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
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ret = EPHIDGET_OK;


	switch (phid->UCD->uid) {
	case PHIDCHUID_HUB0000_HUB_100:
		break;
	case PHIDCHUID_HUB0001_HUB_100:
		break;
	case PHIDCHUID_HUB0004_HUB_100:
		break;
	case PHIDCHUID_HUB0005_HUB_100:
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
	case PHIDCHUID_HUB0000_HUB_100:
		break;
	case PHIDCHUID_HUB0001_HUB_100:
		break;
	case PHIDCHUID_HUB0004_HUB_100:
		break;
	case PHIDCHUID_HUB0005_HUB_100:
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
PhidgetHub_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetHub));
}

API_PRETURN
PhidgetHub_create(PhidgetHubHandle *phidp) {

	CHANNELCREATE_BODY(Hub, PHIDCHCLASS_HUB);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetHub_delete(PhidgetHubHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetHub_setADCCalibrationValues(PhidgetHubHandle ch, double voltageInputGain[6],
  double voltageRatioGain[6]) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_HUB);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCALIBRATIONVALUES, NULL, NULL, "%6G%6G",
	  voltageInputGain, voltageRatioGain);
}

API_PRETURN
PhidgetHub_setFirmwareUpgradeFlag(PhidgetHubHandle ch, int port, uint32_t timeout) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_HUB);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETFIRMWAREUPGRADEFLAG, NULL, NULL, "%d%u",
	  port, timeout);
}

API_PRETURN
PhidgetHub_setPortMode(PhidgetHubHandle ch, int port, PhidgetHub_PortMode mode) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_HUB);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETPORTMODE, NULL, NULL, "%d%d", port,
	  mode);
}

API_PRETURN
PhidgetHub_setPortPower(PhidgetHubHandle ch, int port, int state) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_HUB);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETPORTPOWER, NULL, NULL, "%d%d", port,
	  state);
}
