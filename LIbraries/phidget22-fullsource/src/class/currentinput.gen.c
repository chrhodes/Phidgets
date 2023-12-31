/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

#include "device/advancedservodevice.h"
#include "device/stepperdevice.h"
#include "device/motorcontroldevice.h"
static void CCONV PhidgetCurrentInput_errorHandler(PhidgetChannelHandle ch,
  Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetCurrentInput_bridgeInput(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetCurrentInput_setStatus(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetCurrentInput_getStatus(PhidgetChannelHandle phid,
  BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetCurrentInput_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetCurrentInput_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetCurrentInput_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetCurrentInput {
	struct _PhidgetChannel phid;
	double current;
	double minCurrent;
	double maxCurrent;
	double currentChangeTrigger;
	double minCurrentChangeTrigger;
	double maxCurrentChangeTrigger;
	uint32_t dataInterval;
	uint32_t minDataInterval;
	uint32_t maxDataInterval;
	Phidget_PowerSupply powerSupply;
	PhidgetCurrentInput_OnCurrentChangeCallback CurrentChange;
	void *CurrentChangeCtx;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetCurrentInputHandle ch;
	int version;

	ch = (PhidgetCurrentInputHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 0) {
		logerr("%"PRIphid": bad version %d != 0", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	ch->current = getBridgePacketDoubleByName(bp, "current");
	ch->minCurrent = getBridgePacketDoubleByName(bp, "minCurrent");
	ch->maxCurrent = getBridgePacketDoubleByName(bp, "maxCurrent");
	ch->currentChangeTrigger = getBridgePacketDoubleByName(bp, "currentChangeTrigger");
	ch->minCurrentChangeTrigger = getBridgePacketDoubleByName(bp, "minCurrentChangeTrigger");
	ch->maxCurrentChangeTrigger = getBridgePacketDoubleByName(bp, "maxCurrentChangeTrigger");
	ch->dataInterval = getBridgePacketUInt32ByName(bp, "dataInterval");
	ch->minDataInterval = getBridgePacketUInt32ByName(bp, "minDataInterval");
	ch->maxDataInterval = getBridgePacketUInt32ByName(bp, "maxDataInterval");
	ch->powerSupply = getBridgePacketInt32ByName(bp, "powerSupply");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetCurrentInputHandle ch;

	ch = (PhidgetCurrentInputHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",current=%g"
	  ",minCurrent=%g"
	  ",maxCurrent=%g"
	  ",currentChangeTrigger=%g"
	  ",minCurrentChangeTrigger=%g"
	  ",maxCurrentChangeTrigger=%g"
	  ",dataInterval=%u"
	  ",minDataInterval=%u"
	  ",maxDataInterval=%u"
	  ",powerSupply=%d"
	  ,0 /* class version */
	  ,ch->current
	  ,ch->minCurrent
	  ,ch->maxCurrent
	  ,ch->currentChangeTrigger
	  ,ch->minCurrentChangeTrigger
	  ,ch->maxCurrentChangeTrigger
	  ,ch->dataInterval
	  ,ch->minDataInterval
	  ,ch->maxDataInterval
	  ,ch->powerSupply
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetCurrentInputHandle ch;
	PhidgetReturnCode res;

	ch = (PhidgetCurrentInputHandle)phid;
	res = EPHIDGET_OK;

	switch (bp->vpkt) {
	case BP_SETCHANGETRIGGER:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minCurrentChangeTrigger,
		  ch->maxCurrentChangeTrigger);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->currentChangeTrigger = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "CurrentChangeTrigger");
		break;
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
	case BP_SETPOWERSUPPLY:
		if (!supportedPowerSupply(phid, (Phidget_PowerSupply)getBridgePacketInt32(bp, 0)))
			return (EPHIDGET_INVALIDARG);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->powerSupply = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "PowerSupply");
		break;
	case BP_CURRENTCHANGE:
		ch->current = getBridgePacketDouble(bp, 0);
		FIRECH(ch, CurrentChange, ch->current);
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetAdvancedServoDeviceHandle parentAdvancedServo;
	PhidgetMotorControlDeviceHandle parentMotorControl;
	PhidgetStepperDeviceHandle parentStepper;
	PhidgetCurrentInputHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetCurrentInputHandle)phid;

	ret = EPHIDGET_OK;

	parentAdvancedServo = (PhidgetAdvancedServoDeviceHandle)phid->parent;
	parentMotorControl = (PhidgetMotorControlDeviceHandle)phid->parent;
	parentStepper = (PhidgetStepperDeviceHandle)phid->parent;

	switch (phid->UCD->uid) {
	case PHIDCHUID_1061_CURRENTINPUT_100:
		ch->dataInterval = 256;
		ch->minDataInterval = 32;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 5;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentAdvancedServo->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1061_CURRENTINPUT_200:
		ch->dataInterval = 256;
		ch->minDataInterval = 32;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 5;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentAdvancedServo->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1061_CURRENTINPUT_300:
		ch->dataInterval = 256;
		ch->minDataInterval = 32;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 5;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentAdvancedServo->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1063_CURRENTINPUT_100:
		ch->dataInterval = 256;
		ch->minDataInterval = 8;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 2.492;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentStepper->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1064_CURRENTINPUT_100:
		ch->dataInterval = 256;
		ch->minDataInterval = 32;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 37.9;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentMotorControl->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1065_CURRENTINPUT_100:
		ch->dataInterval = 256;
		ch->minDataInterval = 8;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 80;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentMotorControl->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_1066_CURRENTINPUT_100:
		ch->dataInterval = 256;
		ch->minDataInterval = 32;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 5;
		ch->maxCurrentChangeTrigger = 1;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = parentAdvancedServo->current[ch->phid.index];
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_DAQ1400_CURRENTINPUT_100:
		ch->dataInterval = 250;
		ch->minDataInterval = 20;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 0.02;
		ch->maxCurrentChangeTrigger = 0.016;
		ch->minCurrent = 0.0005;
		ch->minCurrentChangeTrigger = 0;
		ch->current = PUNK_DBL;
		ch->currentChangeTrigger = 0;
		ch->powerSupply = POWER_SUPPLY_12V;
		break;
	case PHIDCHUID_VCP1100_CURRENTINPUT_100:
		ch->dataInterval = 250;
		ch->minDataInterval = 20;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 30;
		ch->maxCurrentChangeTrigger = 60;
		ch->minCurrent = -30;
		ch->minCurrentChangeTrigger = 0;
		ch->current = PUNK_DBL;
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_DCC1000_CURRENTINPUT_100:
		ch->dataInterval = 250;
		ch->minDataInterval = 100;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 25;
		ch->maxCurrentChangeTrigger = 25;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = PUNK_DBL;
		ch->currentChangeTrigger = 0;
		break;
	case PHIDCHUID_DCC1000_CURRENTINPUT_200:
		ch->dataInterval = 250;
		ch->minDataInterval = 100;
		ch->maxDataInterval = 60000;
		ch->maxCurrent = 25;
		ch->maxCurrentChangeTrigger = 25;
		ch->minCurrent = 0;
		ch->minCurrentChangeTrigger = 0;
		ch->current = PUNK_DBL;
		ch->currentChangeTrigger = 0;
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}


	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetCurrentInputHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ch = (PhidgetCurrentInputHandle)phid;
	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_1061_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1061_CURRENTINPUT_200:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1061_CURRENTINPUT_300:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1063_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1064_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1065_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_1066_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_DAQ1400_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETPOWERSUPPLY, NULL, NULL, "%d", ch->powerSupply);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [powerSupply] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_VCP1100_CURRENTINPUT_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
		  ch->currentChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [currentChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_DCC1000_CURRENTINPUT_100:
		break;
	case PHIDCHUID_DCC1000_CURRENTINPUT_200:
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	return (ret);
}

static void CCONV
_fireInitialEvents(PhidgetChannelHandle phid) {
	PhidgetCurrentInputHandle ch;

	ch = (PhidgetCurrentInputHandle)phid;

	if(ch->current != PUNK_DBL)
		FIRECH(ch, CurrentChange, ch->current);

}

static void CCONV
PhidgetCurrentInput_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetCurrentInput));
}

API_PRETURN
PhidgetCurrentInput_create(PhidgetCurrentInputHandle *phidp) {

	CHANNELCREATE_BODY(CurrentInput, PHIDCHCLASS_CURRENTINPUT);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_delete(PhidgetCurrentInputHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetCurrentInput_getCurrent(PhidgetCurrentInputHandle ch, double *current) {

	TESTPTRL(ch);
	TESTPTRL(current);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*current = ch->current;
	if (ch->current == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMinCurrent(PhidgetCurrentInputHandle ch, double *minCurrent) {

	TESTPTRL(ch);
	TESTPTRL(minCurrent);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*minCurrent = ch->minCurrent;
	if (ch->minCurrent == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMaxCurrent(PhidgetCurrentInputHandle ch, double *maxCurrent) {

	TESTPTRL(ch);
	TESTPTRL(maxCurrent);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*maxCurrent = ch->maxCurrent;
	if (ch->maxCurrent == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_setCurrentChangeTrigger(PhidgetCurrentInputHandle ch, double currentChangeTrigger) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
	  currentChangeTrigger));
}

API_PRETURN
PhidgetCurrentInput_getCurrentChangeTrigger(PhidgetCurrentInputHandle ch,
  double *currentChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(currentChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*currentChangeTrigger = ch->currentChangeTrigger;
	if (ch->currentChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMinCurrentChangeTrigger(PhidgetCurrentInputHandle ch,
  double *minCurrentChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(minCurrentChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*minCurrentChangeTrigger = ch->minCurrentChangeTrigger;
	if (ch->minCurrentChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMaxCurrentChangeTrigger(PhidgetCurrentInputHandle ch,
  double *maxCurrentChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(maxCurrentChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*maxCurrentChangeTrigger = ch->maxCurrentChangeTrigger;
	if (ch->maxCurrentChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_setDataInterval(PhidgetCurrentInputHandle ch, uint32_t dataInterval) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETDATAINTERVAL, NULL, NULL, "%u",
	  dataInterval));
}

API_PRETURN
PhidgetCurrentInput_getDataInterval(PhidgetCurrentInputHandle ch, uint32_t *dataInterval) {

	TESTPTRL(ch);
	TESTPTRL(dataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*dataInterval = ch->dataInterval;
	if (ch->dataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMinDataInterval(PhidgetCurrentInputHandle ch, uint32_t *minDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(minDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*minDataInterval = ch->minDataInterval;
	if (ch->minDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_getMaxDataInterval(PhidgetCurrentInputHandle ch, uint32_t *maxDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(maxDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	*maxDataInterval = ch->maxDataInterval;
	if (ch->maxDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_setPowerSupply(PhidgetCurrentInputHandle ch, Phidget_PowerSupply powerSupply) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETPOWERSUPPLY, NULL, NULL, "%d",
	  powerSupply));
}

API_PRETURN
PhidgetCurrentInput_getPowerSupply(PhidgetCurrentInputHandle ch, Phidget_PowerSupply *powerSupply) {

	TESTPTRL(ch);
	TESTPTRL(powerSupply);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1061_CURRENTINPUT_100:
	case PHIDCHUID_1061_CURRENTINPUT_200:
	case PHIDCHUID_1061_CURRENTINPUT_300:
	case PHIDCHUID_1063_CURRENTINPUT_100:
	case PHIDCHUID_1064_CURRENTINPUT_100:
	case PHIDCHUID_1065_CURRENTINPUT_100:
	case PHIDCHUID_1066_CURRENTINPUT_100:
	case PHIDCHUID_VCP1100_CURRENTINPUT_100:
	case PHIDCHUID_DCC1000_CURRENTINPUT_100:
	case PHIDCHUID_DCC1000_CURRENTINPUT_200:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*powerSupply = ch->powerSupply;
	if (ch->powerSupply == (Phidget_PowerSupply)PUNK_ENUM)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetCurrentInput_setOnCurrentChangeHandler(PhidgetCurrentInputHandle ch,
  PhidgetCurrentInput_OnCurrentChangeCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_CURRENTINPUT);

	ch->CurrentChange = fptr;
	ch->CurrentChangeCtx = ctx;

	return (EPHIDGET_OK);
}
