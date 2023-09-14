#ifndef _DEVICES_GEN_H_
#define _DEVICES_GEN_H_

#define PHIDGET_DEVICE_CLASS_COUNT		25
#define PHIDGET_CHANNEL_CLASS_COUNT		38
extern const char *Phid_ClassName[PHIDGET_DEVICE_CLASS_COUNT];
extern const char *Phid_ChannelClassName[PHIDGET_CHANNEL_CLASS_COUNT];


typedef enum {
	PHIDUID_NOTHING = 0,
	PHIDUID_IFKIT488,
	PHIDUID_1000_OLD1,
	PHIDUID_1000_OLD2,
	PHIDUID_1000_NO_ECHO,
	PHIDUID_1000,
	PHIDUID_1001_OLD1,
	PHIDUID_1001_OLD2,
	PHIDUID_1001_NO_ECHO,
	PHIDUID_1001,
	PHIDUID_1002,
	PHIDUID_1008,
	PHIDUID_1011,
	PHIDUID_1012_NO_ECHO,
	PHIDUID_1012_BITBUG,
	PHIDUID_1012,
	PHIDUID_1013_NO_ECHO,
	PHIDUID_1013,
	PHIDUID_1014_NO_ECHO,
	PHIDUID_1014,
	PHIDUID_1015,
	PHIDUID_1016,
	PHIDUID_1017,
	PHIDUID_1018,
	PHIDUID_1023_OLD,
	PHIDUID_1023,
	PHIDUID_1023_2OUTPUT_NO_ECHO,
	PHIDUID_1023_2OUTPUT,
	PHIDUID_1024,
	PHIDUID_1030,
	PHIDUID_1031,
	PHIDUID_1032,
	PHIDUID_1040,
	PHIDUID_1041,
	PHIDUID_1042,
	PHIDUID_1043,
	PHIDUID_1044,
	PHIDUID_1045,
	PHIDUID_1046_GAINBUG,
	PHIDUID_1046,
	PHIDUID_1047,
	PHIDUID_1048,
	PHIDUID_1049,
	PHIDUID_1051_OLD,
	PHIDUID_1051,
	PHIDUID_1051_AD22100,
	PHIDUID_1051_TERMINAL_BLOCKS,
	PHIDUID_1052_OLD,
	PHIDUID_1052_v1,
	PHIDUID_1052_v2,
	PHIDUID_1053,
	PHIDUID_1054,
	PHIDUID_1055,
	PHIDUID_1056,
	PHIDUID_1056_NEG_GAIN,
	PHIDUID_1057,
	PHIDUID_1058,
	PHIDUID_1059,
	PHIDUID_1060,
	PHIDUID_1061,
	PHIDUID_1061_PGOOD_FLAG,
	PHIDUID_1061_CURSENSE_FIX,
	PHIDUID_1062,
	PHIDUID_1063,
	PHIDUID_1064,
	PHIDUID_1065,
	PHIDUID_1066,
	PHIDUID_1067,
	PHIDUID_1202_IFKIT_NO_ECHO,
	PHIDUID_1202_IFKIT,
	PHIDUID_1202_TEXTLCD,
	PHIDUID_1202_IFKIT_FAST,
	PHIDUID_1202_TEXTLCD_BRIGHTNESS,
	PHIDUID_1204,
	PHIDUID_1215,
	PHIDUID_1219,
	PHIDUID_DIGITALINPUT_PORT,
	PHIDUID_DIGITALOUTPUT_PORT,
	PHIDUID_VOLTAGEINPUT_PORT,
	PHIDUID_VOLTAGERATIOINPUT_PORT,
	PHIDUID_ADP1000,
	PHIDUID_ADP1001,
	PHIDUID_DAQ1000,
	PHIDUID_OUT1000,
	PHIDUID_OUT1001,
	PHIDUID_OUT1002,
	PHIDUID_DAQ1200,
	PHIDUID_OUT1100,
	PHIDUID_DAQ1300,
	PHIDUID_DAQ1301,
	PHIDUID_DAQ1400,
	PHIDUID_DAQ1500,
	PHIDUID_VCP1100,
	PHIDUID_DCC1000,
	PHIDUID_DCC1000_POSITIONCONTROL,
	PHIDUID_DCC1001,
	PHIDUID_DCC1002,
	PHIDUID_DCC1003,
	PHIDUID_DCC1100,
	PHIDUID_DST1000,
	PHIDUID_DST1200,
	PHIDUID_ENC1000,
	PHIDUID_HIN1101,
	PHIDUID_HIN1000,
	PHIDUID_HIN1001,
	PHIDUID_HIN1100,
	PHIDUID_HUM1000,
	PHIDUID_LCD1100,
	PHIDUID_LED1000,
	PHIDUID_LUX1000,
	PHIDUID_MOT1100_OLD,
	PHIDUID_MOT1100,
	PHIDUID_MOT1101,
	PHIDUID_PRE1000,
	PHIDUID_RCC1000,
	PHIDUID_REL1000,
	PHIDUID_REL1100,
	PHIDUID_REL1101,
	PHIDUID_SAF1000,
	PHIDUID_SND1000,
	PHIDUID_STC1000,
	PHIDUID_STC1001,
	PHIDUID_STC1002,
	PHIDUID_STC1003,
	PHIDUID_TMP1000,
	PHIDUID_TMP1100,
	PHIDUID_TMP1101,
	PHIDUID_TMP1200,
	PHIDUID_TMP1300,
	PHIDUID_VCP1000,
	PHIDUID_VCP1001,
	PHIDUID_VCP1002,
	PHIDUID_HUB0000,
	PHIDUID_HUB0001,
	PHIDUID_HUB0002,
	PHIDUID_HUB0004,
	PHIDUID_HUB0005,
	PHIDUID_FIRMWARE_UPGRADE_M3_USB,
	PHIDUID_FIRMWARE_UPGRADE_STM32F0,
	PHIDUID_FIRMWARE_UPGRADE_STM8S,
	PHIDUID_FIRMWARE_UPGRADE_M3_SPI,
	PHIDUID_GENERIC,
	PHIDUID_USBSWITCH,
	PHIDUID_GENERICVINT,
	PHIDUID_DICTIONARY,
	PHIDUID_TEXTLED_4x8,
	PHIDUID_TEXTLED_1x8,
	PHIDUID_INTERFACEKIT_2_8_8,
	PHIDUID_POWER,
	PHIDUID_INTERFACEKIT_0_5_7_LCD,
	PHIDUID_INTERFACEKIT_0_32_32,
	PHIDUID_WEIGHTSENSOR,
	PHIDUID_MAXUID
} Phidget_DeviceUID;

typedef enum {
	PHIDCHUID_NOTHING = 0,
	PHIDCHUID_ifkit488_VOLTAGERATIOINPUT_000,
	PHIDCHUID_ifkit488_DIGITALINPUT_000,
	PHIDCHUID_ifkit488_DIGITALOUTPUT_000,
	PHIDCHUID_1000_RCSERVO_OLD1_200,
	PHIDCHUID_1000_RCSERVO_OLD2_200,
	PHIDCHUID_1000_RCSERVO_300,
	PHIDCHUID_1000_RCSERVO_313,
	PHIDCHUID_1001_RCSERVO_OLD1_200,
	PHIDCHUID_1001_RCSERVO_OLD2_200,
	PHIDCHUID_1001_RCSERVO_313,
	PHIDCHUID_1001_RCSERVO_400,
	PHIDCHUID_1002_VOLTAGEOUTPUT_100,
	PHIDCHUID_1008_ACCELEROMETER_000,
	PHIDCHUID_1011_VOLTAGEINPUT_000,
	PHIDCHUID_1011_VOLTAGERATIOINPUT_000,
	PHIDCHUID_1011_DIGITALINPUT_000,
	PHIDCHUID_1011_DIGITALOUTPUT_000,
	PHIDCHUID_1012_DIGITALINPUT_000,
	PHIDCHUID_1012_DIGITALOUTPUT_000,
	PHIDCHUID_1012_DIGITALINPUT_601,
	PHIDCHUID_1012_DIGITALOUTPUT_601,
	PHIDCHUID_1012_DIGITALINPUT_602,
	PHIDCHUID_1012_DIGITALOUTPUT_602,
	PHIDCHUID_1013_VOLTAGEINPUT_000,
	PHIDCHUID_1013_VOLTAGERATIOINPUT_000,
	PHIDCHUID_1013_DIGITALINPUT_000,
	PHIDCHUID_1013_DIGITALOUTPUT_000,
	PHIDCHUID_1018_VOLTAGEINPUT_821,
	PHIDCHUID_1018_VOLTAGERATIOINPUT_821,
	PHIDCHUID_1018_DIGITALINPUT_821,
	PHIDCHUID_1018_DIGITALOUTPUT_821,
	PHIDCHUID_1014_DIGITALOUTPUT_000,
	PHIDCHUID_1014_DIGITALOUTPUT_704,
	PHIDCHUID_1015_CAPACITIVETOUCH_000,
	PHIDCHUID_1016_CAPACITIVETOUCH_000,
	PHIDCHUID_1017_DIGITALOUTPUT_000,
	PHIDCHUID_1018_VOLTAGEINPUT_900,
	PHIDCHUID_1018_VOLTAGERATIOINPUT_900,
	PHIDCHUID_1018_DIGITALINPUT_900,
	PHIDCHUID_1018_DIGITALOUTPUT_900,
	PHIDCHUID_1023_RFID_000,
	PHIDCHUID_1023_RFID_104,
	PHIDCHUID_1023_RFID_200,
	PHIDCHUID_1023_DIGITALOUTPUT_5V_200,
	PHIDCHUID_1023_DIGITALOUTPUT_LED_200,
	PHIDCHUID_1023_DIGITALOUTPUT_ONBOARD_LED_200,
	PHIDCHUID_1023_RFID_201,
	PHIDCHUID_1023_DIGITALOUTPUT_5V_201,
	PHIDCHUID_1023_DIGITALOUTPUT_LED_201,
	PHIDCHUID_1023_DIGITALOUTPUT_ONBOARD_LED_201,
	PHIDCHUID_1024_RFID_100,
	PHIDCHUID_1024_DIGITALOUTPUT_5V_100,
	PHIDCHUID_1024_DIGITALOUTPUT_LED_100,
	PHIDCHUID_1024_DIGITALOUTPUT_ONBOARD_LED_100,
	PHIDCHUID_1030_DIGITALOUTPUT_100,
	PHIDCHUID_1031_DIGITALOUTPUT_100,
	PHIDCHUID_1032_DIGITALOUTPUT_200,
	PHIDCHUID_1040_GPS_000,
	PHIDCHUID_1041_ACCELEROMETER_200,
	PHIDCHUID_1042_ACCELEROMETER_300,
	PHIDCHUID_1042_GYROSCOPE_300,
	PHIDCHUID_1042_MAGNETOMETER_300,
	PHIDCHUID_1042_SPATIAL_300,
	PHIDCHUID_1043_ACCELEROMETER_300,
	PHIDCHUID_1044_ACCELEROMETER_400,
	PHIDCHUID_1044_GYROSCOPE_400,
	PHIDCHUID_1044_MAGNETOMETER_400,
	PHIDCHUID_1044_SPATIAL_400,
	PHIDCHUID_1045_TEMPERATURESENSOR_IR_100,
	PHIDCHUID_1045_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_1046_VOLTAGERATIOINPUT_100,
	PHIDCHUID_1046_VOLTAGERATIOINPUT_102,
	PHIDCHUID_1047_ENCODER_100,
	PHIDCHUID_1047_DIGITALINPUT_100,
	PHIDCHUID_1048_TEMPERATURESENSOR_THERMOCOUPLE_100,
	PHIDCHUID_1048_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_1048_VOLTAGEINPUT_100,
	PHIDCHUID_1049_ACCELEROMETER_000,
	PHIDCHUID_1051_TEMPERATURESENSOR_THERMOCOUPLE_000,
	PHIDCHUID_1051_TEMPERATURESENSOR_IC_000,
	PHIDCHUID_1051_TEMPERATURESENSOR_THERMOCOUPLE_200,
	PHIDCHUID_1051_TEMPERATURESENSOR_IC_200,
	PHIDCHUID_1051_VOLTAGEINPUT_200,
	PHIDCHUID_1051_TEMPERATURESENSOR_THERMOCOUPLE_300,
	PHIDCHUID_1051_TEMPERATURESENSOR_IC_300,
	PHIDCHUID_1051_VOLTAGEINPUT_300,
	PHIDCHUID_1051_TEMPERATURESENSOR_THERMOCOUPLE_400,
	PHIDCHUID_1051_TEMPERATURESENSOR_IC_400,
	PHIDCHUID_1051_VOLTAGEINPUT_400,
	PHIDCHUID_1052_ENCODER_000,
	PHIDCHUID_1052_DIGITALINPUT_000,
	PHIDCHUID_1052_ENCODER_101,
	PHIDCHUID_1052_DIGITALINPUT_101,
	PHIDCHUID_1052_ENCODER_110,
	PHIDCHUID_1052_DIGITALINPUT_110,
	PHIDCHUID_1053_ACCELEROMETER_300,
	PHIDCHUID_1054_FREQUENCYCOUNTER_000,
	PHIDCHUID_1055_IR_100,
	PHIDCHUID_1056_ACCELEROMETER_000,
	PHIDCHUID_1056_GYROSCOPE_000,
	PHIDCHUID_1056_MAGNETOMETER_000,
	PHIDCHUID_1056_SPATIAL_000,
	PHIDCHUID_1056_ACCELEROMETER_200,
	PHIDCHUID_1056_GYROSCOPE_200,
	PHIDCHUID_1056_MAGNETOMETER_200,
	PHIDCHUID_1056_SPATIAL_200,
	PHIDCHUID_1057_ENCODER_300,
	PHIDCHUID_1058_VOLTAGEINPUT_100,
	PHIDCHUID_1058_PHADAPTER_100,
	PHIDCHUID_1059_ACCELEROMETER_400,
	PHIDCHUID_1060_DCMOTOR_100,
	PHIDCHUID_1060_DIGITALINPUT_100,
	PHIDCHUID_1061_RCSERVO_100,
	PHIDCHUID_1061_CURRENTINPUT_100,
	PHIDCHUID_1061_RCSERVO_200,
	PHIDCHUID_1061_CURRENTINPUT_200,
	PHIDCHUID_1061_RCSERVO_300,
	PHIDCHUID_1061_CURRENTINPUT_300,
	PHIDCHUID_1062_STEPPER_100,
	PHIDCHUID_1063_STEPPER_100,
	PHIDCHUID_1063_DIGITALINPUT_100,
	PHIDCHUID_1063_CURRENTINPUT_100,
	PHIDCHUID_1064_DCMOTOR_100,
	PHIDCHUID_1064_CURRENTINPUT_100,
	PHIDCHUID_1065_DCMOTOR_100,
	PHIDCHUID_1065_DIGITALINPUT_100,
	PHIDCHUID_1065_ENCODER_100,
	PHIDCHUID_1065_VOLTAGEINPUT_100,
	PHIDCHUID_1065_VOLTAGEINPUT_SUPPLY_100,
	PHIDCHUID_1065_VOLTAGERATIOINPUT_100,
	PHIDCHUID_1065_CURRENTINPUT_100,
	PHIDCHUID_1066_RCSERVO_100,
	PHIDCHUID_1066_CURRENTINPUT_100,
	PHIDCHUID_1067_STEPPER_200,
	PHIDCHUID_1202_VOLTAGEINPUT_000,
	PHIDCHUID_1202_VOLTAGERATIOINPUT_000,
	PHIDCHUID_1202_DIGITALINPUT_000,
	PHIDCHUID_1202_DIGITALOUTPUT_000,
	PHIDCHUID_1202_VOLTAGEINPUT_120,
	PHIDCHUID_1202_VOLTAGERATIOINPUT_120,
	PHIDCHUID_1202_DIGITALINPUT_120,
	PHIDCHUID_1202_DIGITALOUTPUT_120,
	PHIDCHUID_1202_TEXTLCD_000,
	PHIDCHUID_1202_VOLTAGEINPUT_300,
	PHIDCHUID_1202_VOLTAGERATIOINPUT_300,
	PHIDCHUID_1202_DIGITALINPUT_300,
	PHIDCHUID_1202_DIGITALOUTPUT_300,
	PHIDCHUID_1202_TEXTLCD_200,
	PHIDCHUID_1204_TEXTLCD_000,
	PHIDCHUID_1215_TEXTLCD_000,
	PHIDCHUID_1219_TEXTLCD_000,
	PHIDCHUID_1219_DIGITALINPUT_000,
	PHIDCHUID_1219_DIGITALOUTPUT_000,
	PHIDCHUID_HUB_DIGITALINPUT_100,
	PHIDCHUID_HUB_DIGITALOUTPUT_100,
	PHIDCHUID_HUB_VOLTAGEINPUT_100,
	PHIDCHUID_HUB_VOLTAGERATIOINPUT_100,
	PHIDCHUID_ADP1000_PHSENSOR_100,
	PHIDCHUID_ADP1000_VOLTAGEINPUT_100,
	PHIDCHUID_ADP1001_DATAADAPTER_100,
	PHIDCHUID_DAQ1000_VOLTAGERATIOINPUT_100,
	PHIDCHUID_DAQ1000_VOLTAGEINPUT_100,
	PHIDCHUID_OUT1000_VOLTAGEOUTPUT_100,
	PHIDCHUID_OUT1001_VOLTAGEOUTPUT_100,
	PHIDCHUID_OUT1002_VOLTAGEOUTPUT_100,
	PHIDCHUID_DAQ1200_DIGITALINPUT_100,
	PHIDCHUID_OUT1100_DIGITALOUTPUT_100,
	PHIDCHUID_DAQ1300_DIGITALINPUT_100,
	PHIDCHUID_DAQ1301_DIGITALINPUT_100,
	PHIDCHUID_DAQ1400_VOLTAGEINPUT_100,
	PHIDCHUID_DAQ1400_CURRENTINPUT_100,
	PHIDCHUID_DAQ1400_DIGITALINPUT_100,
	PHIDCHUID_DAQ1400_FREQUENCYCOUNTER_100,
	PHIDCHUID_DAQ1500_VOLTAGERATIOINPUT_100,
	PHIDCHUID_VCP1100_CURRENTINPUT_100,
	PHIDCHUID_DCC1000_DCMOTOR_100,
	PHIDCHUID_DCC1000_ENCODER_100,
	PHIDCHUID_DCC1000_VOLTAGERATIOINPUT_100,
	PHIDCHUID_DCC1000_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_DCC1000_CURRENTINPUT_100,
	PHIDCHUID_DCC1000_DCMOTOR_200,
	PHIDCHUID_DCC1000_ENCODER_200,
	PHIDCHUID_DCC1000_VOLTAGERATIOINPUT_200,
	PHIDCHUID_DCC1000_TEMPERATURESENSOR_IC_200,
	PHIDCHUID_DCC1000_CURRENTINPUT_200,
	PHIDCHUID_DCC1000_MOTORPOSITIONCONTROLLER_200,
	PHIDCHUID_DCC1001_DCMOTOR_100,
	PHIDCHUID_DCC1001_ENCODER_100,
	PHIDCHUID_DCC1001_MOTORPOSITIONCONTROLLER_100,
	PHIDCHUID_DCC1002_DCMOTOR_100,
	PHIDCHUID_DCC1002_ENCODER_100,
	PHIDCHUID_DCC1002_MOTORPOSITIONCONTROLLER_100,
	PHIDCHUID_DCC1003_DCMOTOR_100,
	PHIDCHUID_DCC1100_BLDCMOTOR_100,
	PHIDCHUID_DCC1100_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_DCC1100_MOTORPOSITIONCONTROLLER_100,
	PHIDCHUID_DST1000_DISTANCESENSOR_100,
	PHIDCHUID_DST1200_DISTANCESENSOR_100,
	PHIDCHUID_ENC1000_ENCODER_100,
	PHIDCHUID_HIN1101_ENCODER_100,
	PHIDCHUID_HIN1101_DIGITALINPUT_100,
	PHIDCHUID_HIN1000_CAPACITIVETOUCH_100,
	PHIDCHUID_HIN1001_CAPACITIVETOUCH_BUTTONS_100,
	PHIDCHUID_HIN1001_CAPACITIVETOUCH_WHEEL_100,
	PHIDCHUID_HIN1100_VOLTAGERATIOINPUT_100,
	PHIDCHUID_HIN1100_DIGITALINPUT_100,
	PHIDCHUID_HUM1000_HUMIDITYSENSOR_100,
	PHIDCHUID_HUM1000_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_LCD1100_LCD_100,
	PHIDCHUID_LED1000_DIGITALOUTPUT_100,
	PHIDCHUID_LUX1000_LIGHTSENSOR_100,
	PHIDCHUID_MOT1100_ACCELEROMETER_100,
	PHIDCHUID_MOT1100_ACCELEROMETER_200,
	PHIDCHUID_MOT1101_ACCELEROMETER_100,
	PHIDCHUID_MOT1101_GYROSCOPE_100,
	PHIDCHUID_MOT1101_MAGNETOMETER_100,
	PHIDCHUID_MOT1101_SPATIAL_100,
	PHIDCHUID_PRE1000_PRESSURESENSOR_100,
	PHIDCHUID_RCC1000_RCSERVO_100,
	PHIDCHUID_REL1000_DIGITALOUTPUT_100,
	PHIDCHUID_REL1100_DIGITALOUTPUT_100,
	PHIDCHUID_REL1101_DIGITALOUTPUT_100,
	PHIDCHUID_SAF1000_POWERGUARD_100,
	PHIDCHUID_SAF1000_VOLTAGEINPUT_100,
	PHIDCHUID_SAF1000_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_SND1000_SOUNDSENSOR_100,
	PHIDCHUID_STC1000_STEPPER_100,
	PHIDCHUID_STC1001_STEPPER_100,
	PHIDCHUID_STC1002_STEPPER_100,
	PHIDCHUID_STC1003_STEPPER_100,
	PHIDCHUID_TMP1000_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_TMP1100_TEMPERATURESENSOR_THERMOCOUPLE_100,
	PHIDCHUID_TMP1100_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_TMP1100_VOLTAGEINPUT_100,
	PHIDCHUID_TMP1101_TEMPERATURESENSOR_THERMOCOUPLE_100,
	PHIDCHUID_TMP1101_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_TMP1101_VOLTAGEINPUT_100,
	PHIDCHUID_TMP1200_TEMPERATURESENSOR_RTD_100,
	PHIDCHUID_TMP1200_RESISTANCEINPUT_100,
	PHIDCHUID_TMP1300_TEMPERATURESENSOR_IR_100,
	PHIDCHUID_TMP1300_TEMPERATURESENSOR_IC_100,
	PHIDCHUID_TMP1300_VOLTAGEINPUT_100,
	PHIDCHUID_VCP1000_VOLTAGEINPUT_100,
	PHIDCHUID_VCP1001_VOLTAGEINPUT_100,
	PHIDCHUID_VCP1002_VOLTAGEINPUT_100,
	PHIDCHUID_HUB0000_HUB_100,
	PHIDCHUID_HUB0001_HUB_100,
	PHIDCHUID_HUB0002_MESHDONGLE_100,
	PHIDCHUID_HUB0004_HUB_100,
	PHIDCHUID_HUB0005_HUB_100,
	PHIDCHUID_M3_USB_FIRMWARE_UPGRADE_000,
	PHIDCHUID_STM32F0_FIRMWARE_UPGRADE_100,
	PHIDCHUID_STM8S_FIRMWARE_UPGRADE_100,
	PHIDCHUID_M3_SPI_FIRMWARE_UPGRADE_000,
	PHIDCHUID_USB_GENERIC,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_IN1_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_IN2_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_IN3_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_IN4_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_A1_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_A2_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_A3_100,
	PHIDCHUID_USBSWITCH_DIGITALOUTPUT_A4_100,
	PHIDCHUID_VINT_GENERIC,
	PHIDCHUID_DICTIONARY,
	PHIDCHUID_MAXCHUID
} Phidget_DeviceChannelUID;

typedef enum {

	/*
	* USB Phidgets
	* We own Product IDs 0x30 - 0xAF (48-175)
	*/

	USBID_0x30 = 0x30,	/* 1023, */
	USBID_0x31 = 0x31,	/* 1023, */
	USBID_0x32 = 0x32,	/* 1048, */
	USBID_0x33 = 0x33,	/* 1042, 1044, 1056, */
	USBID_0x34 = 0x34,	/* 1024, */
	USBID_0x35 = 0x35,	/* 1054, */
	USBID_0x36 = 0x36,	/* 1011, */
	USBID_0x37 = 0x37,	/* 1002, */
	USBID_0x38 = 0x38,	/* 1001, */
	USBID_0x39 = 0x39,	/* 1000, */
	USBID_0x3a = 0x3a,	/* 1061, */
	USBID_0x3b = 0x3b,	/* 1046, */
	USBID_0x3c = 0x3c,	/* 1045, */
	USBID_0x3d = 0x3d,	/* 1204, */
	USBID_0x3e = 0x3e,	/* 1065, */
	USBID_0x3f = 0x3f,	/* HUB0000, */
	USBID_0x40 = 0x40,	/* 1014, */
	USBID_0x41 = 0x41,	/* HUB0002, */
	USBID_0x44 = 0x44,	/* 1012, */
	USBID_0x45 = 0x45,	/* 1013, 1013/1018/1019, 1010/1018/1019, */
	USBID_0x48 = 0x48,	/* textled4x8, */
	USBID_0x49 = 0x49,	/* textled1x8, */
	USBID_0x4a = 0x4a,	/* 1030, */
	USBID_0x4b = 0x4b,	/* 1052, */
	USBID_0x4c = 0x4c,	/* 1031, 1032, */
	USBID_0x4d = 0x4d,	/* 1055, */
	USBID_0x4f = 0x4f,	/* 1047, */
	USBID_0x51 = 0x51,	/* ifkit057, */
	USBID_0x52 = 0x52,	/* 1215/1216/1217/1218, */
	USBID_0x53 = 0x53,	/* 1219/1220/1221/1222, */
	USBID_0x58 = 0x58,	/* 1060, */
	USBID_0x59 = 0x59,	/* 1064, */
	USBID_0x60 = 0x60,	/* ifkit3232, */
	USBID_0x70 = 0x70,	/* 1051, */
	USBID_0x71 = 0x71,	/* 1008, 1053, */
	USBID_0x72 = 0x72,	/* 1050, */
	USBID_0x74 = 0x74,	/* 1058, */
	USBID_0x76 = 0x76,	/* 1015, */
	USBID_0x77 = 0x77,	/* 1016, */
	USBID_0x79 = 0x79,	/* 1040, */
	USBID_0x7a = 0x7a,	/* 1062, */
	USBID_0x7b = 0x7b,	/* 1063, 1067, */
	USBID_0x7d = 0x7d,	/* 1202/1203, */
	USBID_0x7e = 0x7e,	/* 1059, */
	USBID_0x7f = 0x7f,	/* 1041, 1043, 1049, */
	USBID_0x80 = 0x80,	/* 1057, */
	USBID_0x81 = 0x81,	/* 1017, */
	USBID_0x82 = 0x82,	/* 1066, */
	USBID_0x98 = 0x98,	/* FIRMWARE_UPGRADE_M3, */
	USBID_0x99 = 0x99,	/* GenericDevice, */
	USBID_0x9a = 0x9a,	/* USBSWITCH, */
	USBID_0x8101 = 0x8101,	/* 1000, */
	USBID_0x8104 = 0x8104,	/* 1001, */
	USBID_0x8200 = 0x8200,	/* ifkit288, */
	USBID_0x8201 = 0x8201,	/* ifkit488, */
	USBID_0x8500 = 0x8500,	/* power, */

	/*
	* VINT Phidgets
	*/

	VINTID_0x1 = 0x1,	/* DIGITALINPUT_PORT, */
	VINTID_0x2 = 0x2,	/* DIGITALOUTPUT_PORT, */
	VINTID_0x3 = 0x3,	/* VOLTAGEINPUT_PORT, */
	VINTID_0x4 = 0x4,	/* VOLTAGERATIOINPUT_PORT, */
	VINTID_0x10 = 0x10,	/* TMP1200, */
	VINTID_0x11 = 0x11,	/* PRE1000, */
	VINTID_0x12 = 0x12,	/* ENC1000, */
	VINTID_0x13 = 0x13,	/* TMP1000, */
	VINTID_0x14 = 0x14,	/* HUM1000, */
	VINTID_0x15 = 0x15,	/* TMP1101, */
	VINTID_0x16 = 0x16,	/* TMP1300, */
	VINTID_0x17 = 0x17,	/* ADP1001, */
	VINTID_0x18 = 0x18,	/* DAQ1500, */
	VINTID_0x19 = 0x19,	/* OUT1100, */
	VINTID_0x1a = 0x1a,	/* REL1100, */
	VINTID_0x1b = 0x1b,	/* REL1101, */
	VINTID_0x1c = 0x1c,	/* DAQ1200, */
	VINTID_0x1d = 0x1d,	/* ADP1000, */
	VINTID_0x1e = 0x1e,	/* VCP1002, */
	VINTID_0x1f = 0x1f,	/* VCP1001, */
	VINTID_0x20 = 0x20,	/* DAQ1300, */
	VINTID_0x21 = 0x21,	/* LUX1000, */
	VINTID_0x22 = 0x22,	/* DAQ1400, */
	VINTID_0x23 = 0x23,	/* SND1000, */
	VINTID_0x24 = 0x24,	/* HIN1000, */
	VINTID_0x25 = 0x25,	/* HIN1100, */
	VINTID_0x26 = 0x26,	/* SAF1000, */
	VINTID_0x27 = 0x27,	/* LED1000, */
	VINTID_0x28 = 0x28,	/* LCD1100, */
	VINTID_0x29 = 0x29,	/* OUT1000, */
	VINTID_0x2a = 0x2a,	/* OUT1001, */
	VINTID_0x2b = 0x2b,	/* OUT1002, */
	VINTID_0x2c = 0x2c,	/* REL1000, */
	VINTID_0x2d = 0x2d,	/* DST1000, */
	VINTID_0x2e = 0x2e,	/* DST1200, */
	VINTID_0x2f = 0x2f,	/* DCC1000, */
	VINTID_0x30 = 0x30,	/* STC1000, */
	VINTID_0x31 = 0x31,	/* RCC1000, */
	VINTID_0x32 = 0x32,	/* DAQ1000, */
	VINTID_0x33 = 0x33,	/* MOT1100, */
	VINTID_0x34 = 0x34,	/* MOT1101, */
	VINTID_0x35 = 0x35,	/* VCP1000, */
	VINTID_0x36 = 0x36,	/* DAQ1301, */
	VINTID_0x37 = 0x37,	/* TMP1100, */
	VINTID_0x38 = 0x38,	/* HIN1001, */
	VINTID_0x40 = 0x40,	/* VCP1100, */
	VINTID_0x41 = 0x41,	/* DCC1100, */
	VINTID_0x43 = 0x43,	/* HIN1101, */
	VINTID_0x44 = 0x44,	/* DCC1001, */
	VINTID_0x45 = 0x45,	/* STC1001, */
	VINTID_0x46 = 0x46,	/* DCC1002, */
	VINTID_0x47 = 0x47,	/* STC1002, */
	VINTID_0x48 = 0x48,	/* STC1003, */
	VINTID_0x49 = 0x49,	/* DCC1003, */
	VINTID_0x999 = 0x999,	/* GenericVINT, */
	VINTID_0xffd = 0xffd,	/* FIRMWARE_UPGRADE_STM32F0, */
	VINTID_0xffe = 0xffe,	/* FIRMWARE_UPGRADE_STM8S, */

	/*
	* Mesh Phidgets
	*/

	MESHID_0x1 = 0x1,	/* HUB0001, */

	/*
	* SPI Phidgets
	*/

	SPIID_0x1 = 0x1,	/* HUB0004, */
	SPIID_0x2 = 0x2,	/* FIRMWARE_UPGRADE_M3, */

	/*
	* Lightning Phidgets
	*/

	LIGHTNINGID_0x42 = 0x42,	/* HUB0005, */

	/*
	* Virtual Phidgets
	*/

	VIRTUALID_0x0 = 0x0,	/* Dictionary, */
	VIRTUALID_0x42 = 0x42,	/* HUB0005, */
} Phidget_DeviceHardwareID;

PhidgetReturnCode createTypedPhidgetChannelHandle(PhidgetChannelHandle * channel,
  Phidget_ChannelClass class);

#endif /* _DEVICES_GEN_H_ */
