/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

static void CCONV PhidgetSoundSensor_errorHandler(PhidgetChannelHandle ch, Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetSoundSensor_bridgeInput(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetSoundSensor_setStatus(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetSoundSensor_getStatus(PhidgetChannelHandle phid,
  BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetSoundSensor_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetSoundSensor_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetSoundSensor_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetSoundSensor {
	struct _PhidgetChannel phid;
	double lastdB;
	uint32_t dataInterval;
	uint32_t minDataInterval;
	uint32_t maxDataInterval;
	double dB;
	double maxdB;
	double dBA;
	double dBC;
	double noiseFloor;
	double octaves[10];
	double SPLChangeTrigger;
	double minSPLChangeTrigger;
	double maxSPLChangeTrigger;
	PhidgetSoundSensor_SPLRange SPLRange;
	PhidgetSoundSensor_OnSPLChangeCallback SPLChange;
	void *SPLChangeCtx;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetSoundSensorHandle ch;
	int version;

	ch = (PhidgetSoundSensorHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 1) {
		logerr("%"PRIphid": bad version %d != 1", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	ch->lastdB = getBridgePacketDoubleByName(bp, "lastdB");
	ch->dataInterval = getBridgePacketUInt32ByName(bp, "dataInterval");
	ch->minDataInterval = getBridgePacketUInt32ByName(bp, "minDataInterval");
	ch->maxDataInterval = getBridgePacketUInt32ByName(bp, "maxDataInterval");
	ch->dB = getBridgePacketDoubleByName(bp, "dB");
	ch->maxdB = getBridgePacketDoubleByName(bp, "maxdB");
	ch->dBA = getBridgePacketDoubleByName(bp, "dBA");
	ch->dBC = getBridgePacketDoubleByName(bp, "dBC");
	ch->noiseFloor = getBridgePacketDoubleByName(bp, "noiseFloor");
	memcpy(&ch->octaves, getBridgePacketDoubleArrayByName(bp, "octaves"), sizeof (double) * 10);
	ch->SPLChangeTrigger = getBridgePacketDoubleByName(bp, "SPLChangeTrigger");
	ch->minSPLChangeTrigger = getBridgePacketDoubleByName(bp, "minSPLChangeTrigger");
	ch->maxSPLChangeTrigger = getBridgePacketDoubleByName(bp, "maxSPLChangeTrigger");
	ch->SPLRange = getBridgePacketInt32ByName(bp, "SPLRange");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetSoundSensorHandle ch;

	ch = (PhidgetSoundSensorHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",lastdB=%g"
	  ",dataInterval=%u"
	  ",minDataInterval=%u"
	  ",maxDataInterval=%u"
	  ",dB=%g"
	  ",maxdB=%g"
	  ",dBA=%g"
	  ",dBC=%g"
	  ",noiseFloor=%g"
	  ",octaves=%10G"
	  ",SPLChangeTrigger=%g"
	  ",minSPLChangeTrigger=%g"
	  ",maxSPLChangeTrigger=%g"
	  ",SPLRange=%d"
	  ,1 /* class version */
	  ,ch->lastdB
	  ,ch->dataInterval
	  ,ch->minDataInterval
	  ,ch->maxDataInterval
	  ,ch->dB
	  ,ch->maxdB
	  ,ch->dBA
	  ,ch->dBC
	  ,ch->noiseFloor
	  ,ch->octaves
	  ,ch->SPLChangeTrigger
	  ,ch->minSPLChangeTrigger
	  ,ch->maxSPLChangeTrigger
	  ,ch->SPLRange
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetSoundSensorHandle ch;
	PhidgetReturnCode res;

	ch = (PhidgetSoundSensorHandle)phid;
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
	case BP_SETCHANGETRIGGER:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minSPLChangeTrigger, ch->maxSPLChangeTrigger);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->SPLChangeTrigger = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "SPLChangeTrigger");
		break;
	case BP_SETSPLRANGE:
		if (!supportedSPLRange(phid, (PhidgetSoundSensor_SPLRange)getBridgePacketInt32(bp, 0)))
			return (EPHIDGET_INVALIDARG);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->SPLRange = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "SPLRange");
		break;
	case BP_DBCHANGE:
		ch->dB = getBridgePacketDouble(bp, 0);
		ch->dBA = getBridgePacketDouble(bp, 1);
		ch->dBC = getBridgePacketDouble(bp, 2);
		memcpy(&ch->octaves, getBridgePacketDoubleArray(bp, 3), sizeof (double) * 10);
		FIRECH(ch, SPLChange, ch->dB, ch->dBA, ch->dBC, ch->octaves);
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetSoundSensorHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetSoundSensorHandle)phid;

	ret = EPHIDGET_OK;


	switch (phid->UCD->uid) {
	case PHIDCHUID_SND1000_SOUNDSENSOR_100:
		ch->dataInterval = 250;
		ch->maxDataInterval = 60000;
		ch->maxdB = 102;
		ch->maxSPLChangeTrigger = 102;
		ch->minDataInterval = 100;
		ch->noiseFloor = 34;
		ch->minSPLChangeTrigger = 0;
		ch->dB = PUNK_DBL;
		ch->dBA = PUNK_DBL;
		ch->dBC = PUNK_DBL;
		ch->octaves[0] = PUNK_DBL;
		ch->octaves[1] = PUNK_DBL;
		ch->octaves[2] = PUNK_DBL;
		ch->octaves[3] = PUNK_DBL;
		ch->octaves[4] = PUNK_DBL;
		ch->octaves[5] = PUNK_DBL;
		ch->octaves[6] = PUNK_DBL;
		ch->octaves[7] = PUNK_DBL;
		ch->octaves[8] = PUNK_DBL;
		ch->octaves[9] = PUNK_DBL;
		ch->SPLRange = SPL_RANGE_102dB;
		ch->SPLChangeTrigger = 0;
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	ch->lastdB = 0;

	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetSoundSensorHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ch = (PhidgetSoundSensorHandle)phid;
	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_SND1000_SOUNDSENSOR_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETSPLRANGE, NULL, NULL, "%d", ch->SPLRange);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [SPLRange] default: %d", phid, ret);
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

}

static void CCONV
PhidgetSoundSensor_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetSoundSensor));
}

API_PRETURN
PhidgetSoundSensor_create(PhidgetSoundSensorHandle *phidp) {

	CHANNELCREATE_BODY(SoundSensor, PHIDCHCLASS_SOUNDSENSOR);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_delete(PhidgetSoundSensorHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetSoundSensor_setDataInterval(PhidgetSoundSensorHandle ch, uint32_t dataInterval) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETDATAINTERVAL, NULL, NULL, "%u",
	  dataInterval));
}

API_PRETURN
PhidgetSoundSensor_getDataInterval(PhidgetSoundSensorHandle ch, uint32_t *dataInterval) {

	TESTPTRL(ch);
	TESTPTRL(dataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*dataInterval = ch->dataInterval;
	if (ch->dataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getMinDataInterval(PhidgetSoundSensorHandle ch, uint32_t *minDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(minDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*minDataInterval = ch->minDataInterval;
	if (ch->minDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getMaxDataInterval(PhidgetSoundSensorHandle ch, uint32_t *maxDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(maxDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*maxDataInterval = ch->maxDataInterval;
	if (ch->maxDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getdB(PhidgetSoundSensorHandle ch, double *dB) {

	TESTPTRL(ch);
	TESTPTRL(dB);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*dB = ch->dB;
	if (ch->dB == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getMaxdB(PhidgetSoundSensorHandle ch, double *maxdB) {

	TESTPTRL(ch);
	TESTPTRL(maxdB);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*maxdB = ch->maxdB;
	if (ch->maxdB == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getdBA(PhidgetSoundSensorHandle ch, double *dBA) {

	TESTPTRL(ch);
	TESTPTRL(dBA);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*dBA = ch->dBA;
	if (ch->dBA == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getdBC(PhidgetSoundSensorHandle ch, double *dBC) {

	TESTPTRL(ch);
	TESTPTRL(dBC);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*dBC = ch->dBC;
	if (ch->dBC == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getNoiseFloor(PhidgetSoundSensorHandle ch, double *noiseFloor) {

	TESTPTRL(ch);
	TESTPTRL(noiseFloor);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*noiseFloor = ch->noiseFloor;
	if (ch->noiseFloor == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getOctaves(PhidgetSoundSensorHandle ch, double (*octaves)[10]) {

	TESTPTRL(ch);
	TESTPTRL(octaves);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	(*octaves)[0] = ch->octaves[0];
	if (ch->octaves[0] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[1] = ch->octaves[1];
	if (ch->octaves[1] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[2] = ch->octaves[2];
	if (ch->octaves[2] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[3] = ch->octaves[3];
	if (ch->octaves[3] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[4] = ch->octaves[4];
	if (ch->octaves[4] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[5] = ch->octaves[5];
	if (ch->octaves[5] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[6] = ch->octaves[6];
	if (ch->octaves[6] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[7] = ch->octaves[7];
	if (ch->octaves[7] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[8] = ch->octaves[8];
	if (ch->octaves[8] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	(*octaves)[9] = ch->octaves[9];
	if (ch->octaves[9] == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_setSPLChangeTrigger(PhidgetSoundSensorHandle ch, double SPLChangeTrigger) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCHANGETRIGGER, NULL, NULL, "%g",
	  SPLChangeTrigger));
}

API_PRETURN
PhidgetSoundSensor_getSPLChangeTrigger(PhidgetSoundSensorHandle ch, double *SPLChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(SPLChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*SPLChangeTrigger = ch->SPLChangeTrigger;
	if (ch->SPLChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getMinSPLChangeTrigger(PhidgetSoundSensorHandle ch, double *minSPLChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(minSPLChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*minSPLChangeTrigger = ch->minSPLChangeTrigger;
	if (ch->minSPLChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_getMaxSPLChangeTrigger(PhidgetSoundSensorHandle ch, double *maxSPLChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(maxSPLChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*maxSPLChangeTrigger = ch->maxSPLChangeTrigger;
	if (ch->maxSPLChangeTrigger == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_setSPLRange(PhidgetSoundSensorHandle ch, PhidgetSoundSensor_SPLRange SPLRange) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETSPLRANGE, NULL, NULL, "%d", SPLRange));
}

API_PRETURN
PhidgetSoundSensor_getSPLRange(PhidgetSoundSensorHandle ch, PhidgetSoundSensor_SPLRange *SPLRange) {

	TESTPTRL(ch);
	TESTPTRL(SPLRange);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);
	TESTATTACHEDL(ch);

	*SPLRange = ch->SPLRange;
	if (ch->SPLRange == (PhidgetSoundSensor_SPLRange)PUNK_ENUM)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetSoundSensor_setOnSPLChangeHandler(PhidgetSoundSensorHandle ch,
  PhidgetSoundSensor_OnSPLChangeCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_SOUNDSENSOR);

	ch->SPLChange = fptr;
	ch->SPLChangeCtx = ctx;

	return (EPHIDGET_OK);
}
