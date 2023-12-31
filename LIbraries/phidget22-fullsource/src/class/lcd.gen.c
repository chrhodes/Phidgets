/* Generated: Mon May 14 2018 09:49:40 GMT-0600 (Mountain Summer Time) */

#include "device/textlcddevice.h"
static void CCONV PhidgetLCD_errorHandler(PhidgetChannelHandle ch, Phidget_ErrorEventCode code);
static PhidgetReturnCode CCONV PhidgetLCD_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetLCD_setStatus(PhidgetChannelHandle phid, BridgePacket *bp);
static PhidgetReturnCode CCONV PhidgetLCD_getStatus(PhidgetChannelHandle phid, BridgePacket **bp);
static PhidgetReturnCode CCONV PhidgetLCD_initAfterOpen(PhidgetChannelHandle phid);
static PhidgetReturnCode CCONV PhidgetLCD_setDefaults(PhidgetChannelHandle phid);
static void CCONV PhidgetLCD_fireInitialEvents(PhidgetChannelHandle phid);

struct _PhidgetLCD {
	struct _PhidgetChannel phid;
	uint8_t fontWidth[3];
	uint8_t fontHeight[3];
	double backlight;
	double minBacklight;
	double maxBacklight;
	double contrast;
	double minContrast;
	double maxContrast;
	int cursorBlink;
	int cursorOn;
	int frameBuffer;
	int height;
	PhidgetLCD_ScreenSize screenSize;
	int sleeping;
	int width;
};

static PhidgetReturnCode CCONV
_setStatus(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetLCDHandle ch;
	int version;

	ch = (PhidgetLCDHandle)phid;

	version = getBridgePacketUInt32ByName(bp, "_class_version_");
	if (version != 2) {
		logerr("%"PRIphid": bad version %d != 2", phid, version);
		return (EPHIDGET_BADVERSION);
	}

	memcpy(&ch->fontWidth, getBridgePacketUInt8ArrayByName(bp, "fontWidth"), sizeof (uint8_t) * 3);
	memcpy(&ch->fontHeight, getBridgePacketUInt8ArrayByName(bp, "fontHeight"), sizeof (uint8_t) * 3);
	ch->backlight = getBridgePacketDoubleByName(bp, "backlight");
	ch->minBacklight = getBridgePacketDoubleByName(bp, "minBacklight");
	ch->maxBacklight = getBridgePacketDoubleByName(bp, "maxBacklight");
	ch->contrast = getBridgePacketDoubleByName(bp, "contrast");
	ch->minContrast = getBridgePacketDoubleByName(bp, "minContrast");
	ch->maxContrast = getBridgePacketDoubleByName(bp, "maxContrast");
	ch->cursorBlink = getBridgePacketInt32ByName(bp, "cursorBlink");
	ch->cursorOn = getBridgePacketInt32ByName(bp, "cursorOn");
	ch->frameBuffer = getBridgePacketInt32ByName(bp, "frameBuffer");
	ch->height = getBridgePacketInt32ByName(bp, "height");
	ch->screenSize = getBridgePacketInt32ByName(bp, "screenSize");
	ch->sleeping = getBridgePacketInt32ByName(bp, "sleeping");
	ch->width = getBridgePacketInt32ByName(bp, "width");

	return (EPHIDGET_OK);
}

static PhidgetReturnCode CCONV
_getStatus(PhidgetChannelHandle phid, BridgePacket **bp) {
	PhidgetLCDHandle ch;

	ch = (PhidgetLCDHandle)phid;

	return (createBridgePacket(bp, 0, "_class_version_=%u"
	  ",fontWidth=%3R"
	  ",fontHeight=%3R"
	  ",backlight=%g"
	  ",minBacklight=%g"
	  ",maxBacklight=%g"
	  ",contrast=%g"
	  ",minContrast=%g"
	  ",maxContrast=%g"
	  ",cursorBlink=%d"
	  ",cursorOn=%d"
	  ",frameBuffer=%d"
	  ",height=%d"
	  ",screenSize=%d"
	  ",sleeping=%d"
	  ",width=%d"
	  ,2 /* class version */
	  ,ch->fontWidth
	  ,ch->fontHeight
	  ,ch->backlight
	  ,ch->minBacklight
	  ,ch->maxBacklight
	  ,ch->contrast
	  ,ch->minContrast
	  ,ch->maxContrast
	  ,ch->cursorBlink
	  ,ch->cursorOn
	  ,ch->frameBuffer
	  ,ch->height
	  ,ch->screenSize
	  ,ch->sleeping
	  ,ch->width
	));
}

static PhidgetReturnCode CCONV
_bridgeInput(PhidgetChannelHandle phid, BridgePacket *bp) {
	PhidgetLCDHandle ch;
	PhidgetReturnCode res;

	ch = (PhidgetLCDHandle)phid;
	res = EPHIDGET_OK;

	switch (bp->vpkt) {
	case BP_SETCHARACTERBITMAP:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_CLEAR:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_COPY:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_DRAWLINE:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_DRAWPIXEL:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_DRAWRECT:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_FLUSH:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_INITIALIZE:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_SAVEFRAMEBUFFER:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_WRITEBITMAP:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_WRITETEXT:
		res = DEVBRIDGEINPUT(phid, bp);
		break;
	case BP_SETBACKLIGHT:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minBacklight, ch->maxBacklight);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->backlight = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "Backlight");
		break;
	case BP_SETCONTRAST:
		TESTRANGE(getBridgePacketDouble(bp, 0), ch->minContrast, ch->maxContrast);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->contrast = getBridgePacketDouble(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "Contrast");
		break;
	case BP_SETCURSORBLINK:
		TESTBOOL(getBridgePacketInt32(bp, 0));
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->cursorBlink = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "CursorBlink");
		break;
	case BP_SETCURSORON:
		TESTBOOL(getBridgePacketInt32(bp, 0));
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->cursorOn = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "CursorOn");
		break;
	case BP_SETFRAMEBUFFER:
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->frameBuffer = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "FrameBuffer");
		break;
	case BP_SETSCREENSIZE:
		if (!supportedLCDScreenSize(phid, (PhidgetLCD_ScreenSize)getBridgePacketInt32(bp, 0)))
			return (EPHIDGET_INVALIDARG);
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->screenSize = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "ScreenSize");
		break;
	case BP_SETSLEEP:
		TESTBOOL(getBridgePacketInt32(bp, 0));
		res = DEVBRIDGEINPUT(phid, bp);
		if (res != EPHIDGET_OK) {
			logerr("%"PRIphid": DEVBRIDGEINPUT() failed: %d", phid, res);
			break;
		}
		ch->sleeping = getBridgePacketInt32(bp, 0);
		if (bridgePacketIsFromNet(bp))
			FIRE_PROPERTYCHANGE(ch, "Sleeping");
		break;
	default:
		logerr("%"PRIphid": unsupported bridge packet:0x%x", phid, bp->vpkt);
		res = EPHIDGET_UNSUPPORTED;
	}

	return (res);
}

static PhidgetReturnCode CCONV
_initAfterOpen(PhidgetChannelHandle phid) {
	PhidgetLCDHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);
	ch = (PhidgetLCDHandle)phid;

	ret = EPHIDGET_OK;


	switch (phid->UCD->uid) {
	case PHIDCHUID_1202_TEXTLCD_000:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.502;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->cursorBlink = 0;
		ch->cursorOn = 0;
		ch->height = 2;
		ch->width = 20;
		ch->screenSize = SCREEN_SIZE_2x20;
		break;
	case PHIDCHUID_1202_TEXTLCD_200:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.491;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->cursorBlink = 0;
		ch->cursorOn = 0;
		ch->height = 2;
		ch->width = 20;
		ch->screenSize = SCREEN_SIZE_2x20;
		break;
	case PHIDCHUID_1204_TEXTLCD_000:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.785;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->cursorBlink = 0;
		ch->cursorOn = 0;
		ch->height = 0;
		ch->width = 0;
		ch->screenSize = PUNK_ENUM;
		break;
	case PHIDCHUID_1215_TEXTLCD_000:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.5;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->cursorBlink = 0;
		ch->cursorOn = 0;
		ch->height = 2;
		ch->width = 20;
		ch->screenSize = SCREEN_SIZE_2x20;
		break;
	case PHIDCHUID_1219_TEXTLCD_000:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.5;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->cursorBlink = 0;
		ch->cursorOn = 0;
		ch->height = 2;
		ch->width = 20;
		ch->screenSize = SCREEN_SIZE_2x20;
		break;
	case PHIDCHUID_LCD1100_LCD_100:
		ch->backlight = 0;
		ch->minBacklight = 0;
		ch->maxBacklight = 1;
		ch->contrast = 0.25;
		ch->minContrast = 0;
		ch->maxContrast = 1;
		ch->frameBuffer = 0;
		ch->height = 64;
		ch->width = 128;
		ch->screenSize = SCREEN_SIZE_64x128;
		ch->sleeping = 1;
		break;
	default:
		MOS_PANIC("Unsupported Channel");
	}

	memset(ch->fontWidth, '\0', 3);
	memset(ch->fontHeight, '\0', 3);

	return (ret);
}

static PhidgetReturnCode CCONV
_setDefaults(PhidgetChannelHandle phid) {
	PhidgetLCDHandle ch;
	PhidgetReturnCode ret;

	TESTPTRL(phid);

	ch = (PhidgetLCDHandle)phid;
	ret = EPHIDGET_OK;

	switch (phid->UCD->uid) {
	case PHIDCHUID_1202_TEXTLCD_000:
		break;
	case PHIDCHUID_1202_TEXTLCD_200:
		break;
	case PHIDCHUID_1204_TEXTLCD_000:
		break;
	case PHIDCHUID_1215_TEXTLCD_000:
		break;
	case PHIDCHUID_1219_TEXTLCD_000:
		break;
	case PHIDCHUID_LCD1100_LCD_100:
		ret = bridgeSendToDevice(phid, BP_SETCONTRAST, NULL, NULL, "%g", ch->contrast);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [contrast] default: %d", phid, ret);
			break;
		}
		ret = bridgeSendToDevice(phid, BP_SETFRAMEBUFFER, NULL, NULL, "%d", ch->frameBuffer);
		if (ret != EPHIDGET_OK) {
			logerr("%"PRIphid": failed to set [frameBuffer] default: %d", phid, ret);
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
PhidgetLCD_free(PhidgetChannelHandle *ch) {

	mos_free(*ch, sizeof (struct _PhidgetLCD));
}

API_PRETURN
PhidgetLCD_create(PhidgetLCDHandle *phidp) {

	CHANNELCREATE_BODY(LCD, PHIDCHCLASS_LCD);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_delete(PhidgetLCDHandle *phidp) {

	return (Phidget_delete((PhidgetHandle *)phidp));
}

API_PRETURN
PhidgetLCD_clear(PhidgetLCDHandle ch) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_CLEAR, NULL, NULL, NULL);
}

API_VRETURN
PhidgetLCD_clear_async(PhidgetLCDHandle ch, Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_CLEAR, fptr, ctx, NULL);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_copy(PhidgetLCDHandle ch, int sourceFramebuffer, int destFramebuffer, int sourceX1,
  int sourceY1, int sourceX2, int sourceY2, int destX, int destY, int inverted) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_COPY, NULL, NULL, "%d%d%d%d%d%d%d%d%d",
	  sourceFramebuffer, destFramebuffer, sourceX1, sourceY1, sourceX2, sourceY2, destX, destY, inverted);
}

API_VRETURN
PhidgetLCD_copy_async(PhidgetLCDHandle ch, int sourceFramebuffer, int destFramebuffer, int sourceX1,
  int sourceY1, int sourceX2, int sourceY2, int destX, int destY, int inverted, Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_COPY, fptr, ctx, "%d%d%d%d%d%d%d%d%d",
	  sourceFramebuffer, destFramebuffer, sourceX1, sourceY1, sourceX2, sourceY2, destX, destY, inverted);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_drawLine(PhidgetLCDHandle ch, int x1, int y1, int x2, int y2) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWLINE, NULL, NULL, "%d%d%d%d", x1, y1, x2,
	  y2);
}

API_VRETURN
PhidgetLCD_drawLine_async(PhidgetLCDHandle ch, int x1, int y1, int x2, int y2, Phidget_AsyncCallback fptr,
  void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWLINE, fptr, ctx, "%d%d%d%d", x1, y1, x2,
	  y2);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_drawPixel(PhidgetLCDHandle ch, int x, int y, PhidgetLCD_PixelState pixelState) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWPIXEL, NULL, NULL, "%d%d%d", x, y,
	  pixelState);
}

API_VRETURN
PhidgetLCD_drawPixel_async(PhidgetLCDHandle ch, int x, int y, PhidgetLCD_PixelState pixelState,
  Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWPIXEL, fptr, ctx, "%d%d%d", x, y,
	  pixelState);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_drawRect(PhidgetLCDHandle ch, int x1, int y1, int x2, int y2, int filled, int inverted) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWRECT, NULL, NULL, "%d%d%d%d%d%d", x1, y1,
	  x2, y2, filled, inverted);
}

API_VRETURN
PhidgetLCD_drawRect_async(PhidgetLCDHandle ch, int x1, int y1, int x2, int y2, int filled, int inverted,
  Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_DRAWRECT, fptr, ctx, "%d%d%d%d%d%d", x1, y1,
	  x2, y2, filled, inverted);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_flush(PhidgetLCDHandle ch) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_FLUSH, NULL, NULL, NULL);
}

API_VRETURN
PhidgetLCD_flush_async(PhidgetLCDHandle ch, Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_FLUSH, fptr, ctx, NULL);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_setFontSize(PhidgetLCDHandle ch, PhidgetLCD_Font font, int width, int height) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETFONTSIZE, NULL, NULL, "%d%d%d", font,
	  width, height);
}

API_PRETURN
PhidgetLCD_initialize(PhidgetLCDHandle ch) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_INITIALIZE, NULL, NULL, NULL);
}

API_PRETURN
PhidgetLCD_saveFrameBuffer(PhidgetLCDHandle ch, int frameBuffer) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SAVEFRAMEBUFFER, NULL, NULL, "%d",
	  frameBuffer);
}

API_VRETURN
PhidgetLCD_saveFrameBuffer_async(PhidgetLCDHandle ch, int frameBuffer, Phidget_AsyncCallback fptr,
  void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SAVEFRAMEBUFFER, fptr, ctx, "%d",
	  frameBuffer);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_writeText(PhidgetLCDHandle ch, PhidgetLCD_Font font, int xPosition, int yPosition,
  const char *text) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return bridgeSendToDevice((PhidgetChannelHandle)ch, BP_WRITETEXT, NULL, NULL, "%d%d%d%s", font,
	  xPosition, yPosition, text);
}

API_VRETURN
PhidgetLCD_writeText_async(PhidgetLCDHandle ch, PhidgetLCD_Font font, int xPosition, int yPosition,
  const char *text, Phidget_AsyncCallback fptr, void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_WRITETEXT, fptr, ctx, "%d%d%d%s", font,
	  xPosition, yPosition, text);

	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_setBacklight(PhidgetLCDHandle ch, double backlight) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETBACKLIGHT, NULL, NULL, "%g",
	  backlight));
}

API_PRETURN
PhidgetLCD_getBacklight(PhidgetLCDHandle ch, double *backlight) {

	TESTPTRL(ch);
	TESTPTRL(backlight);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*backlight = ch->backlight;
	if (ch->backlight == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getMinBacklight(PhidgetLCDHandle ch, double *minBacklight) {

	TESTPTRL(ch);
	TESTPTRL(minBacklight);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*minBacklight = ch->minBacklight;
	if (ch->minBacklight == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getMaxBacklight(PhidgetLCDHandle ch, double *maxBacklight) {

	TESTPTRL(ch);
	TESTPTRL(maxBacklight);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*maxBacklight = ch->maxBacklight;
	if (ch->maxBacklight == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setContrast(PhidgetLCDHandle ch, double contrast) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCONTRAST, NULL, NULL, "%g", contrast));
}

API_PRETURN
PhidgetLCD_getContrast(PhidgetLCDHandle ch, double *contrast) {

	TESTPTRL(ch);
	TESTPTRL(contrast);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*contrast = ch->contrast;
	if (ch->contrast == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getMinContrast(PhidgetLCDHandle ch, double *minContrast) {

	TESTPTRL(ch);
	TESTPTRL(minContrast);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*minContrast = ch->minContrast;
	if (ch->minContrast == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getMaxContrast(PhidgetLCDHandle ch, double *maxContrast) {

	TESTPTRL(ch);
	TESTPTRL(maxContrast);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*maxContrast = ch->maxContrast;
	if (ch->maxContrast == (double)PUNK_DBL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setCursorBlink(PhidgetLCDHandle ch, int cursorBlink) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCURSORBLINK, NULL, NULL, "%d",
	  cursorBlink));
}

API_PRETURN
PhidgetLCD_getCursorBlink(PhidgetLCDHandle ch, int *cursorBlink) {

	TESTPTRL(ch);
	TESTPTRL(cursorBlink);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_LCD1100_LCD_100:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*cursorBlink = ch->cursorBlink;
	if (ch->cursorBlink == (int)PUNK_BOOL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setCursorOn(PhidgetLCDHandle ch, int cursorOn) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETCURSORON, NULL, NULL, "%d", cursorOn));
}

API_PRETURN
PhidgetLCD_getCursorOn(PhidgetLCDHandle ch, int *cursorOn) {

	TESTPTRL(ch);
	TESTPTRL(cursorOn);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_LCD1100_LCD_100:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*cursorOn = ch->cursorOn;
	if (ch->cursorOn == (int)PUNK_BOOL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setFrameBuffer(PhidgetLCDHandle ch, int frameBuffer) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETFRAMEBUFFER, NULL, NULL, "%d",
	  frameBuffer));
}

API_VRETURN
PhidgetLCD_setFrameBuffer_async(PhidgetLCDHandle ch, int frameBuffer, Phidget_AsyncCallback fptr,
  void *ctx) {
	PhidgetReturnCode res;

	if (ch == NULL) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_INVALIDARG);
		return;
	}
	if (ch->phid.class != PHIDCHCLASS_LCD) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_WRONGDEVICE);
		return;
	}
	if (!ISATTACHED(ch)) {
		if (fptr) fptr((PhidgetHandle)ch, ctx, EPHIDGET_NOTATTACHED);
		return;
	}

	res = bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETFRAMEBUFFER, fptr, ctx, "%d",
	  frameBuffer);
	if (res != EPHIDGET_OK && fptr != NULL)
		fptr((PhidgetHandle)ch, ctx, res);
}

API_PRETURN
PhidgetLCD_getFrameBuffer(PhidgetLCDHandle ch, int *frameBuffer) {

	TESTPTRL(ch);
	TESTPTRL(frameBuffer);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1202_TEXTLCD_000:
	case PHIDCHUID_1202_TEXTLCD_200:
	case PHIDCHUID_1204_TEXTLCD_000:
	case PHIDCHUID_1215_TEXTLCD_000:
	case PHIDCHUID_1219_TEXTLCD_000:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*frameBuffer = ch->frameBuffer;
	if (ch->frameBuffer == (int)PUNK_INT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getHeight(PhidgetLCDHandle ch, int *height) {

	TESTPTRL(ch);
	TESTPTRL(height);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*height = ch->height;
	if (ch->height == (int)PUNK_INT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setScreenSize(PhidgetLCDHandle ch, PhidgetLCD_ScreenSize screenSize) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETSCREENSIZE, NULL, NULL, "%d",
	  screenSize));
}

API_PRETURN
PhidgetLCD_getScreenSize(PhidgetLCDHandle ch, PhidgetLCD_ScreenSize *screenSize) {

	TESTPTRL(ch);
	TESTPTRL(screenSize);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*screenSize = ch->screenSize;
	if (ch->screenSize == (PhidgetLCD_ScreenSize)PUNK_ENUM)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_setSleeping(PhidgetLCDHandle ch, int sleeping) {

	TESTPTRL(ch);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	return (bridgeSendToDevice((PhidgetChannelHandle)ch, BP_SETSLEEP, NULL, NULL, "%d", sleeping));
}

API_PRETURN
PhidgetLCD_getSleeping(PhidgetLCDHandle ch, int *sleeping) {

	TESTPTRL(ch);
	TESTPTRL(sleeping);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	switch (ch->phid.UCD->uid) {
	case PHIDCHUID_1202_TEXTLCD_000:
	case PHIDCHUID_1202_TEXTLCD_200:
	case PHIDCHUID_1204_TEXTLCD_000:
	case PHIDCHUID_1215_TEXTLCD_000:
	case PHIDCHUID_1219_TEXTLCD_000:
		return (EPHIDGET_UNSUPPORTED);
	default:
		break;
	}

	*sleeping = ch->sleeping;
	if (ch->sleeping == (int)PUNK_BOOL)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}

API_PRETURN
PhidgetLCD_getWidth(PhidgetLCDHandle ch, int *width) {

	TESTPTRL(ch);
	TESTPTRL(width);
	TESTCHANNELCLASSL(ch, PHIDCHCLASS_LCD);
	TESTATTACHEDL(ch);

	*width = ch->width;
	if (ch->width == (int)PUNK_INT32)
		return (EPHIDGET_UNKNOWNVAL);
	return (EPHIDGET_OK);
}
