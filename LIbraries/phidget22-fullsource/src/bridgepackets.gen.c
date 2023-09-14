#include "bridgepackets.gen.h"
#include "constantsinternal.h"

int
bridgePacketSupportsDataGram(bridgepacket_t pkt) {

	switch (pkt) {
	case BP_ACCELERATIONCHANGE:
	case BP_ANGULARRATEUPDATE:
	case BP_CURRENTCHANGE:
	case BP_DBCHANGE:
	case BP_DISTANCECHANGE:
	case BP_FIELDSTRENGTHCHANGE:
	case BP_HUMIDITYCHANGE:
	case BP_ILLUMINANCECHANGE:
	case BP_PRESSURECHANGE:
	case BP_RESISTANCECHANGE:
	case BP_SPATIALDATA:
	case BP_TEMPERATURECHANGE:
	case BP_TOUCHINPUTVALUECHANGE:
	case BP_VOLTAGECHANGE:
	case BP_VOLTAGERATIOCHANGE:
	case BP_TIME:
	case BP_DATE:
	case BP_HEADINGCHANGE:
	case BP_PROGRESSCHANGE:
	case BP_SENSORCHANGE:
	case BP_PHCHANGE:
		return (1);
	default:
		return (0);
	}
}

bridgepacketinfo_t bridgepacketinfo[] = {
	{ "BP_SETSTATUS", 0}, /* 0x0 */
	{ "BP_ACCELERATIONCHANGE", 0}, /* 0x1 */
	{ "BP_ANGULARRATEUPDATE", 0}, /* 0x2 */
	{ "BP_BACKEMFCHANGE", 0}, /* 0x3 */
	{ "BP_CLEAR", BP_FLAG_NOFORWARD}, /* 0x4 */
	{ "BP_CODE", 0}, /* 0x5 */
	{ "BP_COPY", BP_FLAG_NOFORWARD}, /* 0x6 */
	{ "BP_COUNTCHANGE", 0}, /* 0x7 */
	{ "BP_CURRENTCHANGE", 0}, /* 0x8 */
	{ "BP_DATA", 0}, /* 0x9 */
	{ "BP_DATAINTERVALCHANGE", 0}, /* 0xa */
	{ "BP_DBCHANGE", 0}, /* 0xb */
	{ "BP_DISTANCECHANGE", 0}, /* 0xc */
	{ "BP_DRAWLINE", BP_FLAG_NOFORWARD}, /* 0xd */
	{ "BP_DRAWPIXEL", BP_FLAG_NOFORWARD}, /* 0xe */
	{ "BP_DRAWRECT", BP_FLAG_NOFORWARD}, /* 0xf */
	{ "BP_DUTYCYCLECHANGE", 0}, /* 0x10 */
	{ "BP_ERROREVENT", 0}, /* 0x11 */
	{ "BP_FIELDSTRENGTHCHANGE", 0}, /* 0x12 */
	{ "BP_FLUSH", BP_FLAG_NOFORWARD}, /* 0x13 */
	{ "BP_FREQUENCYCHANGE", 0}, /* 0x14 */
	{ "BP_FREQUENCYDATA", 0}, /* 0x15 */
	{ "BP_HUMIDITYCHANGE", 0}, /* 0x16 */
	{ "BP_ILLUMINANCECHANGE", 0}, /* 0x17 */
	{ "BP_INITIALIZE", BP_FLAG_NOFORWARD}, /* 0x18 */
	{ "BP_LEARN", 0}, /* 0x19 */
	{ "BP_MANCHESTER", 0}, /* 0x1a */
	{ "BP_MINDATAINTERVALCHANGE", 0}, /* 0x1b */
	{ "BP_PACKET", 0}, /* 0x1c */
	{ "BP_POSITIONCHANGE", 0}, /* 0x1d */
	{ "BP_POSITIONFIXSTATUSCHANGE", 0}, /* 0x1e */
	{ "BP_PRESSURECHANGE", 0}, /* 0x1f */
	{ "BP_RAWDATA", 0}, /* 0x20 */
	{ "BP_REPEAT", 0}, /* 0x21 */
	{ "BP_OPENRESET", 0}, /* 0x22 */
	{ "BP_RESETCORRECTIONPARAMETERS", BP_FLAG_NOFORWARD}, /* 0x23 */
	{ "BP_RESISTANCECHANGE", 0}, /* 0x24 */
	{ "BP_SAVECORRECTIONPARAMETERS", BP_FLAG_NOFORWARD}, /* 0x25 */
	{ "BP_SAVEFRAMEBUFFER", BP_FLAG_NOFORWARD}, /* 0x26 */
	{ "BP_SENDPACKET", BP_FLAG_NOFORWARD}, /* 0x27 */
	{ "BP_SETACCELERATION", 0}, /* 0x28 */
	{ "BP_SETANTENNAON", 0}, /* 0x29 */
	{ "BP_SETBACKEMFSENSINGSTATE", 0}, /* 0x2a */
	{ "BP_SETBACKLIGHT", 0}, /* 0x2b */
	{ "BP_SETBRAKINGDUTYCYCLE", 0}, /* 0x2c */
	{ "BP_SETBRIDGEGAIN", 0}, /* 0x2d */
	{ "BP_SETCHANGETRIGGER", 0}, /* 0x2e */
	{ "BP_SETCHARACTERBITMAP", BP_FLAG_NOFORWARD}, /* 0x2f */
	{ "BP_SETCONTRAST", 0}, /* 0x30 */
	{ "BP_SETCONTROLMODE", 0}, /* 0x31 */
	{ "BP_SETCORRECTIONPARAMETERS", BP_FLAG_NOFORWARD}, /* 0x32 */
	{ "BP_SETCURRENTLIMIT", 0}, /* 0x33 */
	{ "BP_SETCURSORBLINK", 0}, /* 0x34 */
	{ "BP_SETCURSORON", 0}, /* 0x35 */
	{ "BP_SETDATAINTERVAL", 0}, /* 0x36 */
	{ "BP_SETDUTYCYCLE", 0}, /* 0x37 */
	{ "BP_SETENABLED", 0}, /* 0x38 */
	{ "BP_SETENGAGED", 0}, /* 0x39 */
	{ "BP_SETFANMODE", 0}, /* 0x3a */
	{ "BP_SETFILTERTYPE", 0}, /* 0x3b */
	{ "BP_SETFIRMWAREUPGRADEFLAG", BP_FLAG_NOFORWARD}, /* 0x3c */
	{ "BP_SETFONTSIZE", BP_FLAG_NOFORWARD}, /* 0x3d */
	{ "BP_SETFRAMEBUFFER", 0}, /* 0x3e */
	{ "BP_SETHOLDINGCURRENTLIMIT", 0}, /* 0x3f */
	{ "BP_SETINPUTMODE", 0}, /* 0x40 */
	{ "BP_SETIOMODE", 0}, /* 0x41 */
	{ "BP_SETSENSITIVITY", 0}, /* 0x42 */
	{ "BP_SETLEDCURRENTLIMIT", 0}, /* 0x43 */
	{ "BP_SETLEDFORWARDVOLTAGE", 0}, /* 0x44 */
	{ "BP_SETMAXPULSEWIDTH", 0}, /* 0x45 */
	{ "BP_SETMINPULSEWIDTH", 0}, /* 0x46 */
	{ "BP_SETOVERVOLTAGE", 0}, /* 0x47 */
	{ "BP_SETPORTMODE", BP_FLAG_NOFORWARD}, /* 0x48 */
	{ "BP_SETPORTPOWER", BP_FLAG_NOFORWARD}, /* 0x49 */
	{ "BP_SETPOWERSUPPLY", 0}, /* 0x4a */
	{ "BP_SETSONARQUIETMODE", 0}, /* 0x4b */
	{ "BP_SETRTDTYPE", 0}, /* 0x4c */
	{ "BP_SETRTDWIRESETUP", 0}, /* 0x4d */
	{ "BP_SETSCREENSIZE", 0}, /* 0x4e */
	{ "BP_SETSENSORTYPE", 0}, /* 0x4f */
	{ "BP_SETSLEEP", 0}, /* 0x50 */
	{ "BP_SETSPEEDRAMPINGSTATE", 0}, /* 0x51 */
	{ "BP_SETSTATE", 0}, /* 0x52 */
	{ "BP_SETTARGETPOSITION", 0}, /* 0x53 */
	{ "BP_SETTHERMOCOUPLETYPE", 0}, /* 0x54 */
	{ "BP_SETVELOCITYLIMIT", 0}, /* 0x55 */
	{ "BP_SETVOLTAGE", 0}, /* 0x56 */
	{ "BP_SETVOLTAGERANGE", 0}, /* 0x57 */
	{ "BP_SONARUPDATE", 0}, /* 0x58 */
	{ "BP_SPATIALDATA", 0}, /* 0x59 */
	{ "BP_STATECHANGE", 0}, /* 0x5a */
	{ "BP_STOPPED", 0}, /* 0x5b */
	{ "BP_TAG", 0}, /* 0x5c */
	{ "BP_TAGLOST", 0}, /* 0x5d */
	{ "BP_TARGETPOSITIONREACHED", 0}, /* 0x5e */
	{ "BP_TEMPERATURECHANGE", 0}, /* 0x5f */
	{ "BP_TOUCHINPUTVALUECHANGE", 0}, /* 0x60 */
	{ "BP_TRANSMIT", BP_FLAG_NOFORWARD}, /* 0x61 */
	{ "BP_TRANSMITRAW", BP_FLAG_NOFORWARD}, /* 0x62 */
	{ "BP_TRANSMITREPEAT", BP_FLAG_NOFORWARD}, /* 0x63 */
	{ "BP_VELOCITYCHANGE", 0}, /* 0x64 */
	{ "BP_VOLTAGECHANGE", 0}, /* 0x65 */
	{ "BP_VOLTAGERATIOCHANGE", 0}, /* 0x66 */
	{ "BP_WRITE", BP_FLAG_NOFORWARD}, /* 0x67 */
	{ "BP_WRITEBITMAP", BP_FLAG_NOFORWARD}, /* 0x68 */
	{ "BP_WRITETEXT", BP_FLAG_NOFORWARD}, /* 0x69 */
	{ "BP_ZERO", BP_FLAG_NOFORWARD}, /* 0x6a */
	{ "BP_SETCALIBRATIONVALUES", BP_FLAG_NOFORWARD}, /* 0x6b */
	{ "BP_TIME", 0}, /* 0x6c */
	{ "BP_DATE", 0}, /* 0x6d */
	{ "BP_HEADINGCHANGE", 0}, /* 0x6e */
	{ "BP_CLOSERESET", 0}, /* 0x6f */
	{ "BP_SENDFIRMWARE", BP_FLAG_NOFORWARD}, /* 0x70 */
	{ "BP_PROGRESSCHANGE", 0}, /* 0x71 */
	{ "BP_DEVICEINFO", 0}, /* 0x72 */
	{ "BP_SENSORCHANGE", 0}, /* 0x73 */
	{ "BP_SETSPLRANGE", 0}, /* 0x74 */
	{ "BP_DATAIN", 0}, /* 0x75 */
	{ "BP_DATAOUT", BP_FLAG_NOFORWARD}, /* 0x76 */
	{ "BP_SETCURRENTREGULATORGAIN", 0}, /* 0x77 */
	{ "BP_SETDEADBAND", 0}, /* 0x78 */
	{ "BP_BRAKINGSTRENGTHCHANGE", 0}, /* 0x79 */
	{ "BP_SETSENSORVALUECHANGETRIGGER", 0}, /* 0x7a */
	{ "BP_DICTIONARYADD", BP_FLAG_NOFORWARD}, /* 0x7b */
	{ "BP_DICTIONARYADDED", 0}, /* 0x7c */
	{ "BP_DICTIONARYUPDATE", BP_FLAG_NOFORWARD}, /* 0x7d */
	{ "BP_DICTIONARYUPDATED", 0}, /* 0x7e */
	{ "BP_DICTIONARYREMOVE", BP_FLAG_NOFORWARD}, /* 0x7f */
	{ "BP_DICTIONARYREMOVED", 0}, /* 0x80 */
	{ "BP_DICTIONARYGET", BP_FLAG_NOFORWARD}, /* 0x81 */
	{ "BP_DICTIONARYSET", BP_FLAG_NOFORWARD}, /* 0x82 */
	{ "BP_DICTIONARYREMOVEALL", BP_FLAG_NOFORWARD}, /* 0x83 */
	{ "BP_DICTIONARYSCAN", BP_FLAG_NOFORWARD}, /* 0x84 */
	{ "BP_PHCHANGE", 0}, /* 0x85 */
	{ "BP_SETCORRECTIONTEMPERATURE", 0}, /* 0x86 */
	{ "BP_SETKP", 0}, /* 0x87 */
	{ "BP_SETKD", 0}, /* 0x88 */
	{ "BP_TOUCHINPUTEND", 0}, /* 0x89 */
	{ "BP_REBOOTFIRMWAREUPGRADE", BP_FLAG_NOFORWARD}, /* 0x8a */
	{ "BP_REBOOT", BP_FLAG_NOFORWARD}, /* 0x8b */
	{ "BP_WRITELABEL", BP_FLAG_NOFORWARD}, /* 0x8c */
	{ "BP_SETSTALLVELOCITY", 0}, /* 0x8d */
	{ "BP_SETKI", 0}, /* 0x8e */
	{ "BP_ENABLE", 0}, /* 0x8f */
	{ (void *)0, 0 }
};
