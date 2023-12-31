/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

#include "device/interfacekitdevice.h"
static void CCONV PhidgetCapacitiveTouch_errorHandler(PhidgetChannelHandle ch,
  Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetCapacitiveTouch_bridgeInput(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetCapacitiveTouch_setStatus(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetCapacitiveTouch_getStatus(PhidgetChannelHandle phid,
  BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetCapacitiveTouch_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetCapacitiveTouch_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetCapacitiveTouch_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetCapacitiveTouch {
	struct _PhidgetChannel phid;
	uint32_t dataInterval;
	uint32_t minDataInterval;
	uint32_t maxDataInterval;
	double sensitivity;
	double minSensitivity;
	double maxSensitivity;
	int isTouched;
	double touchValue;
	double minTouchValue;
	double maxTouchValue;
	double touchValueChangeTrigger;
	double minTouchValueChangeTrigger;
	double maxTouchValueChangeTrigger;
	PhidgetCapacitiveTouch_OnTouchCallback Touch;
	void *TouchCtx;
	PhidgetCapacitiveTouch_OnTouchEndCallback TouchEnd;
	void *TouchEndCtx;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetCapacitiveTouchHandle ch;
	int version;

	ch = (PhidgetCapacitiveTouchHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 2) {
		logerr("%"PRIphid": bad version %d != 2", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	ch->dataInterval = getBridgePacketUInt32ByName(bp, "dataInterval");
	ch->minDataInterval = getBridgePacketUInt32ByName(bp, "minDataInterval");
	ch->maxDataInterval = getBridgePacketUInt32ByName(bp, "maxDataInterval");
	ch->sensitivity = getBridgePacketDoubleByName(bp, "sensitivity");
	ch->minSensitivity = getBridgePacketDoubleByName(bp, "minSensitivity");
	ch->maxSensitivity = getBridgePacketDoubleByName(bp, "maxSensitivity");
	ch->isTouched = getBridgePacketInt32ByName(bp, "isTouched");
	ch->touchValue = getBridgePacketDoubleByName(bp, "touchValue");
	ch->minTouchValue = getBridgePacketDoubleByName(bp, "minTouchValue");
	ch->maxTouchValue = getBridgePacketDoubleByName(bp, "maxTouchValue");
	ch->touchValueChangeTrigger = getBridgePacketDoubleByName(bp, "touchValueChangeTrigger");
	ch->minTouchValueChangeTrigger = getBridgePacketDoubleByName(bp, "minTouchValueChangeTrigger");
	ch->maxTouchValueChangeTrigger = getBridgePacketDoubleByName(bp, "maxTouchValueChangeTrigger");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetCapacitiveTouchHandle ch;

	ch = (PhidgetCapacitiveTouchHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",dataInterval=%u"
	  ",minDataInterval=%u"
	  ",maxDataInterval=%u"
	  ",sensitivity=%g"
	  ",minSensitivity=%g"
	  ",maxSensitivity=%g"
	  ",isTouched=%d"
	  ",touchValue=%g"
	  ",minTouchValue=%g"
	  ",maxTouchValue=%g"
	  ",touchValueChangeTrigger=%g"
	  ",minTouchValueChangeTrigger=%g"
	  ",maxTouchValueChangeTrigger=%g"
	  ,2 /* class version */
	  ,ch->dataInterval
	  ,ch->minDataInterval
	  ,ch->maxDataInterval
	  ,ch->sensitivity
	  ,ch->minSensitivity
	  ,ch->maxSensitivity
	  ,ch->isTouched
	  ,ch->touchValue
	  ,ch->minTouchValue
	  ,ch->maxTouchValue
	  ,ch->touchValueChangeTrigger
	  ,ch->minTouchValueChangeTrigger
	  ,ch->maxTouchValueChangeTrigger
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetCapacitiveTouchHandle ch;
	PhidgetReturnCode res;

	ch = (PhidgetCapacitiveTouchHandle)phid;
	res = EPHIDGET_OK;

	switch (bp->vpkt) {
	case BP_SETDATAINTERVAL:
		TESTRANGE(getBridgePacketUInt32(bp, 0), ch->minDataInterval, ch->maxDataInterval);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->dataInterval = getBridgePacketUInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "DataInterval");
		break;
	case BP_SETSENSITIVITY:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minSensitivity, ch->maxSensitivity);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->sensitivity = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "Sensitivity");
		break;
	case BP_SETCHANGETRIGGER:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minTouchValueChangeTrigger,
		  ch->maxTouchValueChangeTrigger);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->touchValueChangeTrigger = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "TouchValueChangeTrigger");
		break;
	case BP_TOUCHINPUTVALUECHANGE:
		ch->touchValue = getBridgePacketDouble(bp, 0);
		FIRECH(ch, Touch, ch->touchValue);
		break;
	case BP_TOUCHINPUTEND:
		FIRECH0(ch, TouchEnd);
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetInterfaceKitDeviceHandle parentInterfaceKit;
	PhidgetCapacitiveTouchHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetCapacitiveTouchHandle)phid;

	ret = EPHIDGET_OK;

	parentInterfaceKit = (PhidgetInterfaceKitDeviceHandle)phid->parent;

	switch (phid->UCD->uid) {
	case PHIDCHUID_1015_CAPACITIVETOUCH_000:
		ch->touchValueChangeTrigger = 0.005;
		ch->minTouchValueChangeTrigger = 0;
		ch->maxTouchValueChangeTrigger = 1;
		ch->dataInterval = 60;
		ch->minDataInterval = 60;
		ch->maxDataInterval = 1000;
		ch->maxTouchValue = 1;
		ch->minTouchValue = 0;
		ch->touchValue = parentInterfaceKit->touchValue[ch->phid.index];
		ch->isTouched = parentInterfaceKit->isTouched[ch->phid.index];
		break;
	case PHIDCHUID_1016_CAPACITIVETOUCH_000:
		ch->touchValueChangeTrigger = 0.005;
		ch->minTouchValueChangeTrigger = 0;
		ch->maxTouchValueChangeTrigger = 1;
		ch->dataInterval = 60;
		ch->minDataInterval = 60;
		ch->maxDataInterval = 1000;
		ch->maxTouchValue = 1;
		ch->minTouchValue = 0;
		ch->touchValue = parentInterfaceKit->touchValue[ch->phid.index];
		ch->isTouched = parentInterfaceKit->isTouched[ch->phid.index];
		break;
	case PHIDCHUID_HIN1000_CAPACITIVETOUCH_100:
		ch->touchValueChangeTrigger = 0;
		ch->minTouchValueChangeTrigger = 0;
		ch->maxTouchValueChangeTrigger = 1;
		ch->dataInterval = 25;
		ch->minDataInterval = 25;
		ch->maxDataInterval = 1000;
		ch->sensitivity = 0.2;
		ch->minSensitivity = 0;
		ch->maxSensitivity = 1;
		ch->maxTouchValue = 1;
		ch->minTouchValue = 0;
		ch->touchValue = PUNK_DBL;
		ch->isTouched = PUNK_BOOL;
		break;
	case PHIDCHUID_HIN1001_CAPACITIVETOUCH_BUTTONS_100:
		ch->touchValueChangeTrigger = 0;
		ch->minTouchValueChangeTrigger = 0;
		ch->maxTouchValueChangeTrigger = 0.5;
		ch->dataInterval = 20;
		ch->minDataInterval = 20;
		ch->maxDataInterval = 250;
		ch->sensitivity = 0.5;
		ch->minSensitivity = 0;
		ch->maxSensitivity = 1;
		ch->maxTouchValue = 1;
		ch->minTouchValue = 0;
		ch->touchValue = PUNK_DBL;
		ch->isTouched = PUNK_BOOL;
		break;
	case PHIDCHUID_HIN1001_CAPACITIVETOUCH_WHEEL_100:
		ch->touchValueChangeTrigger = 0;
		ch->minTouchValueChangeTrigger = 0;
		ch->maxTouchValueChangeTrigger = 0.5;
		ch->dataInterval = 20;
		ch->minDataInterval = 20;
		ch->maxDataInterval = 250;
		ch->sensitivity = 0.7;
		ch->minSensitivity = 0;
		ch->maxSensitivity = 1;
		ch->maxTouchValue = 1;
		ch->minTouchValue = 0;
		ch->touchValue = PUNK_DBL;
		ch->isTouched = PUNK_BOOL;
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}


	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetCapacitiveTouchHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ch = (PhidgetCapacitiveTouchHandle)phid;
	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_1015_CAPACITIVETOUCH_000:
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->touchValueChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [touchValueChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1016_CAPACITIVETOUCH_000:
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->touchValueChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [touchValueChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_HIN1000_CAPACITIVETOUCH_100:
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->touchValueChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [touchValueChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETSENSITIVITY, NULL, NULL, "%g", ch->sensitivity);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [sensitivity] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_HIN1001_CAPACITIVETOUCH_BUTTONS_100:
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->touchValueChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [touchValueChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETSENSITIVITY, NULL, NULL, "%g", ch->sensitivity);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [sensitivity] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_HIN1001_CAPACITIVETOUCH_WHEEL_100:
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->touchValueChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [touchValueChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETSENSITIVITY, NULL, NULL, "%g", ch->sensitivity);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [sensitivity] default: %d", phid, ret);
			break;
		}
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	return (ret);
}

static void CCONV
_fireInitialEvents(PhidgetChannelHandle phid) {
	PhidgetCapacitiveTouchHandle ch;

	ch = (PhidgetCapacitiveTouchHandle)phid;

	if(ch->touchValue != PUNK_DBL)
		FIRECH(ch, Touch, ch->touchValue);

}

static void CCONV
PhidgetCapacitiveTouch_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetCapacitiveTouch));
}

API_PRETURN
PhidgetCapacitiveTouch_create(PhidgetCapacitiveTouchHandle *phidp) {

	CHANNELCREATE_BODY(CapacitiveTouch, PHIDCHCLASS_CAPACITIVETOUCH);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_delete(PhidgetCapacitiveTouchHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetCapacitiveTouch_setDataInterval(PhidgetCapacitiveTouchHandle ch, uint32_t dataInterval) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETDATAINTERVAL, NULL, NULL, "%u",
	  dataInterval));
}

API_PRETURN
PhidgetCapacitiveTouch_getDataInterval(PhidgetCapacitiveTouchHandle ch, uint32_t *dataInterval) {

	TESTPTRL(ch);
	TESTPTRL(dataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*dataInterval = ch->dataInterval;
	if (ch->dataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMinDataInterval(PhidgetCapacitiveTouchHandle ch, uint32_t *minDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(minDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*minDataInterval = ch->minDataInterval;
	if (ch->minDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMaxDataInterval(PhidgetCapacitiveTouchHandle ch, uint32_t *maxDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(maxDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*maxDataInterval = ch->maxDataInterval;
	if (ch->maxDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_setSensitivity(PhidgetCapacitiveTouchHandle ch, double sensitivity) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETSENSITIVITY, NULL, NULL, "%g",
	  sensitivity));
}

API_PRETURN
PhidgetCapacitiveTouch_getSensitivity(PhidgetCapacitiveTouchHandle ch, double *sensitivity) {

	TESTPTRL(ch);
	TESTPTRL(sensitivity);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1015_CAPACITIVETOUCH_000:
	case PHIDCHUID_1016_CAPACITIVETOUCH_000:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*sensitivity = ch->sensitivity;
	if (ch->sensitivity == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMinSensitivity(PhidgetCapacitiveTouchHandle ch, double *minSensitivity) {

	TESTPTRL(ch);
	TESTPTRL(minSensitivity);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1015_CAPACITIVETOUCH_000:
	case PHIDCHUID_1016_CAPACITIVETOUCH_000:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*minSensitivity = ch->minSensitivity;
	if (ch->minSensitivity == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMaxSensitivity(PhidgetCapacitiveTouchHandle ch, double *maxSensitivity) {

	TESTPTRL(ch);
	TESTPTRL(maxSensitivity);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1015_CAPACITIVETOUCH_000:
	case PHIDCHUID_1016_CAPACITIVETOUCH_000:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*maxSensitivity = ch->maxSensitivity;
	if (ch->maxSensitivity == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getIsTouched(PhidgetCapacitiveTouchHandle ch, int *isTouched) {

	TESTPTRL(ch);
	TESTPTRL(isTouched);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*isTouched = ch->isTouched;
	if (ch->isTouched == (int)PUNK_BOOL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getTouchValue(PhidgetCapacitiveTouchHandle ch, double *touchValue) {

	TESTPTRL(ch);
	TESTPTRL(touchValue);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*touchValue = ch->touchValue;
	if (ch->touchValue == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMinTouchValue(PhidgetCapacitiveTouchHandle ch, double *minTouchValue) {

	TESTPTRL(ch);
	TESTPTRL(minTouchValue);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*minTouchValue = ch->minTouchValue;
	if (ch->minTouchValue == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMaxTouchValue(PhidgetCapacitiveTouchHandle ch, double *maxTouchValue) {

	TESTPTRL(ch);
	TESTPTRL(maxTouchValue);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*maxTouchValue = ch->maxTouchValue;
	if (ch->maxTouchValue == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_setTouchValueChangeTrigger(PhidgetCapacitiveTouchHandle ch,
  double touchValueChangeTrigger) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
	  touchValueChangeTrigger));
}

API_PRETURN
PhidgetCapacitiveTouch_getTouchValueChangeTrigger(PhidgetCapacitiveTouchHandle ch,
  double *touchValueChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(touchValueChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*touchValueChangeTrigger = ch->touchValueChangeTrigger;
	if (ch->touchValueChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMinTouchValueChangeTrigger(PhidgetCapacitiveTouchHandle ch,
  double *minTouchValueChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(minTouchValueChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*minTouchValueChangeTrigger = ch->minTouchValueChangeTrigger;
	if (ch->minTouchValueChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_getMaxTouchValueChangeTrigger(PhidgetCapacitiveTouchHandle ch,
  double *maxTouchValueChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(maxTouchValueChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);
	TESTATTACHEDL(ch);

	*maxTouchValueChangeTrigger = ch->maxTouchValueChangeTrigger;
	if (ch->maxTouchValueChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_setOnTouchHandler(PhidgetCapacitiveTouchHandle ch,
  PhidgetCapacitiveTouch_OnTouchCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);

	ch->Touch = fptr;
	ch->TouchCtx = ctx;

	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCapacitiveTouch_setOnTouchEndHandler(PhidgetCapacitiveTouchHandle ch,
  PhidgetCapacitiveTouch_OnTouchEndCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CAPACITIVETOUCH);

	ch->TouchEnd = fptr;
	ch->TouchEndCtx = ctx;

	return (EPHIDGET_OK);
}
