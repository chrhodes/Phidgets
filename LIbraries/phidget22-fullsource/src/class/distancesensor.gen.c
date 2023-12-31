/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

static void CCONV PhidgetDistanceSensor_errorHandler(PhidgetChannelHandle ch,
  Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetDistanceSensor_bridgeInput(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetDistanceSensor_setStatus(PhidgetChannelHandle phid,
  BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetDistanceSensor_getStatus(PhidgetChannelHandle phid,
  BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetDistanceSensor_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetDistanceSensor_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetDistanceSensor_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetDistanceSensor {
	struct _PhidgetChannel phid;
	uint32_t amplitudes[8];
	uint32_t distances[8];
	uint32_t count;
	uint32_t dataInterval;
	uint32_t minDataInterval;
	uint32_t maxDataInterval;
	uint32_t distance;
	uint32_t minDistance;
	uint32_t maxDistance;
	uint32_t distanceChangeTrigger;
	uint32_t minDistanceChangeTrigger;
	uint32_t maxDistanceChangeTrigger;
	int sonarQuietMode;
	PhidgetDistanceSensor_OnDistanceChangeCallback DistanceChange;
	void *DistanceChangeCtx;
	PhidgetDistanceSensor_OnSonarReflectionsUpdateCallback SonarReflectionsUpdate;
	void *SonarReflectionsUpdateCtx;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetDistanceSensorHandle ch;
	int version;

	ch = (PhidgetDistanceSensorHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 1) {
		logerr("%"PRIphid": bad version %d != 1", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	memcpy(&ch->amplitudes, getBridgePacketUInt32ArrayByName(bp, "amplitudes"), sizeof (uint32_t) * 8);
	memcpy(&ch->distances, getBridgePacketUInt32ArrayByName(bp, "distances"), sizeof (uint32_t) * 8);
	ch->count = getBridgePacketUInt32ByName(bp, "count");
	ch->dataInterval = getBridgePacketUInt32ByName(bp, "dataInterval");
	ch->minDataInterval = getBridgePacketUInt32ByName(bp, "minDataInterval");
	ch->maxDataInterval = getBridgePacketUInt32ByName(bp, "maxDataInterval");
	ch->distance = getBridgePacketUInt32ByName(bp, "distance");
	ch->minDistance = getBridgePacketUInt32ByName(bp, "minDistance");
	ch->maxDistance = getBridgePacketUInt32ByName(bp, "maxDistance");
	ch->distanceChangeTrigger = getBridgePacketUInt32ByName(bp, "distanceChangeTrigger");
	ch->minDistanceChangeTrigger = getBridgePacketUInt32ByName(bp, "minDistanceChangeTrigger");
	ch->maxDistanceChangeTrigger = getBridgePacketUInt32ByName(bp, "maxDistanceChangeTrigger");
	ch->sonarQuietMode = getBridgePacketInt32ByName(bp, "sonarQuietMode");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetDistanceSensorHandle ch;

	ch = (PhidgetDistanceSensorHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",amplitudes=%8U"
	  ",distances=%8U"
	  ",count=%u"
	  ",dataInterval=%u"
	  ",minDataInterval=%u"
	  ",maxDataInterval=%u"
	  ",distance=%u"
	  ",minDistance=%u"
	  ",maxDistance=%u"
	  ",distanceChangeTrigger=%u"
	  ",minDistanceChangeTrigger=%u"
	  ",maxDistanceChangeTrigger=%u"
	  ",sonarQuietMode=%d"
	  ,1 /* class version */
	  ,ch->amplitudes
	  ,ch->distances
	  ,ch->count
	  ,ch->dataInterval
	  ,ch->minDataInterval
	  ,ch->maxDataInterval
	  ,ch->distance
	  ,ch->minDistance
	  ,ch->maxDistance
	  ,ch->distanceChangeTrigger
	  ,ch->minDistanceChangeTrigger
	  ,ch->maxDistanceChangeTrigger
	  ,ch->sonarQuietMode
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetDistanceSensorHandle ch;
	PhidgetReturnCode res;

	ch = (PhidgetDistanceSensorHandle)phid;
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
		TESTRANGE(getBridgePacketUInt32(bp, 0), ch->minDistanceChangeTrigger,
		  ch->maxDistanceChangeTrigger);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->distanceChangeTrigger = getBridgePacketUInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "DistanceChangeTrigger");
		break;
	case BP_SETSONARQUIETMODE:
		TESTBOOL(getBridgePacketInt32(bp, 0));
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->sonarQuietMode = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "SonarQuietMode");
		break;
	case BP_DISTANCECHANGE:
		ch->distance = getBridgePacketUInt32(bp, 0);
		FIRECH(ch, DistanceChange, ch->distance);
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetDistanceSensorHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetDistanceSensorHandle)phid;

	ret = EPHIDGET_OK;


	switch (phid->UCD->uid) {
	case PHIDCHUID_DST1000_DISTANCESENSOR_100:
		ch->dataInterval = 250;
		ch->maxDataInterval = 60000;
		ch->maxDistance = 200;
		ch->maxDistanceChangeTrigger = 200;
		ch->minDataInterval = 100;
		ch->minDistance = 0;
		ch->minDistanceChangeTrigger = 0;
		ch->distance = PUNK_UINT32;
		ch->distanceChangeTrigger = 0;
		break;
	case PHIDCHUID_DST1200_DISTANCESENSOR_100:
		ch->dataInterval = 250;
		ch->maxDataInterval = 60000;
		ch->maxDistance = 10000;
		ch->maxDistanceChangeTrigger = 10000;
		ch->minDataInterval = 100;
		ch->minDistance = 40;
		ch->minDistanceChangeTrigger = 0;
		ch->distance = PUNK_UINT32;
		ch->distanceChangeTrigger = 0;
		ch->sonarQuietMode = 1;
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	memset(ch->amplitudes, '\0', 8);
	memset(ch->distances, '\0', 8);
	ch->count = 0;

	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetDistanceSensorHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ch = (PhidgetDistanceSensorHandle)phid;
	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_DST1000_DISTANCESENSOR_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%u",
		  ch->distanceChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [distanceChangeTrigger] default: %d", phid, ret);
			break;
		}
		break;
	case PHIDCHUID_DST1200_DISTANCESENSOR_100:
		ret = bridgeSendToDevice(phid, BP_SETDATAINTERVAL, NULL, NULL, "%u", ch->dataInterval);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [dataInterval] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETCHANGETRIGGER, NULL, NULL, "%u",
		  ch->distanceChangeTrigger);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [distanceChangeTrigger] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETSONARQUIETMODE, NULL, NULL, "%d", ch->sonarQuietMode);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [sonarQuietMode] default: %d", phid, ret);
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
	PhidgetDistanceSensorHandle ch;

	ch = (PhidgetDistanceSensorHandle)phid;

	if(ch->distance != PUNK_UINT32)
		FIRECH(ch, DistanceChange, ch->distance);

}

static void CCONV
PhidgetDistanceSensor_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetDistanceSensor));
}

API_PRETURN
PhidgetDistanceSensor_create(PhidgetDistanceSensorHandle *phidp) {

	CHANNELCREATE_BODY(DistanceSensor, PHIDCHCLASS_DISTANCESENSOR);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_delete(PhidgetDistanceSensorHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetDistanceSensor_setDataInterval(PhidgetDistanceSensorHandle ch, uint32_t dataInterval) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETDATAINTERVAL, NULL, NULL, "%u",
	  dataInterval));
}

API_PRETURN
PhidgetDistanceSensor_getDataInterval(PhidgetDistanceSensorHandle ch, uint32_t *dataInterval) {

	TESTPTRL(ch);
	TESTPTRL(dataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*dataInterval = ch->dataInterval;
	if (ch->dataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMinDataInterval(PhidgetDistanceSensorHandle ch, uint32_t *minDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(minDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*minDataInterval = ch->minDataInterval;
	if (ch->minDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMaxDataInterval(PhidgetDistanceSensorHandle ch, uint32_t *maxDataInterval) {

	TESTPTRL(ch);
	TESTPTRL(maxDataInterval);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*maxDataInterval = ch->maxDataInterval;
	if (ch->maxDataInterval == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getDistance(PhidgetDistanceSensorHandle ch, uint32_t *distance) {

	TESTPTRL(ch);
	TESTPTRL(distance);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*distance = ch->distance;
	if (ch->distance == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMinDistance(PhidgetDistanceSensorHandle ch, uint32_t *minDistance) {

	TESTPTRL(ch);
	TESTPTRL(minDistance);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*minDistance = ch->minDistance;
	if (ch->minDistance == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMaxDistance(PhidgetDistanceSensorHandle ch, uint32_t *maxDistance) {

	TESTPTRL(ch);
	TESTPTRL(maxDistance);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*maxDistance = ch->maxDistance;
	if (ch->maxDistance == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_setDistanceChangeTrigger(PhidgetDistanceSensorHandle ch,
  uint32_t distanceChangeTrigger) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCHANGETRIGGER, NULL, NULL, "%u",
	  distanceChangeTrigger));
}

API_PRETURN
PhidgetDistanceSensor_getDistanceChangeTrigger(PhidgetDistanceSensorHandle ch,
  uint32_t *distanceChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(distanceChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*distanceChangeTrigger = ch->distanceChangeTrigger;
	if (ch->distanceChangeTrigger == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMinDistanceChangeTrigger(PhidgetDistanceSensorHandle ch,
  uint32_t *minDistanceChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(minDistanceChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*minDistanceChangeTrigger = ch->minDistanceChangeTrigger;
	if (ch->minDistanceChangeTrigger == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_getMaxDistanceChangeTrigger(PhidgetDistanceSensorHandle ch,
  uint32_t *maxDistanceChangeTrigger) {

	TESTPTRL(ch);
	TESTPTRL(maxDistanceChangeTrigger);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	*maxDistanceChangeTrigger = ch->maxDistanceChangeTrigger;
	if (ch->maxDistanceChangeTrigger == (uint32_t)PUNK_UINT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_setSonarQuietMode(PhidgetDistanceSensorHandle ch, int sonarQuietMode) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETSONARQUIETMODE, NULL, NULL, "%d",
	  sonarQuietMode));
}

API_PRETURN
PhidgetDistanceSensor_getSonarQuietMode(PhidgetDistanceSensorHandle ch, int *sonarQuietMode) {

	TESTPTRL(ch);
	TESTPTRL(sonarQuietMode);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_DST1000_DISTANCESENSOR_100:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*sonarQuietMode = ch->sonarQuietMode;
	if (ch->sonarQuietMode == (int)PUNK_BOOL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_setOnDistanceChangeHandler(PhidgetDistanceSensorHandle ch,
  PhidgetDistanceSensor_OnDistanceChangeCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);

	ch->DistanceChange = fptr;
	ch->DistanceChangeCtx = ctx;

	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetDistanceSensor_setOnSonarReflectionsUpdateHandler(PhidgetDistanceSensorHandle ch,
  PhidgetDistanceSensor_OnSonarReflectionsUpdateCallback fptr, void *ctx) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_DISTANCESENSOR);

	ch->SonarReflectionsUpdate = fptr;
	ch->SonarReflectionsUpdateCtx = ctx;

	return (EPHIDGET_OK);
}
