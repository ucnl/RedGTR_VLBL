using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedGTR_VLBL
{
    public enum DeviceType
    {
        DEV_REDBASE = 0, // Surface GNSS buoy
        DEV_REDNODE = 1, // positioning. Underwater-based node
        DEV_REDNAV  = 2,  // positioning. Underwater-based diver navigator
        DEV_REDGTR  = 3,  // communication. General transmitter/receier
        DEV_INVALID
    }

    public enum LocError
    {
        LOC_ERR_NO_ERROR              = 0,
        LOC_ERR_INVALID_SYNTAX        = 1,
        LOC_ERR_UNSUPPORTED           = 2,
        LOC_ERR_TRANSMITTER_BUSY      = 3,
        LOC_ERR_ARGUMENT_OUT_OF_RANGE = 4,
        LOC_ERR_INVALID_OPERATION     = 5,
        LOC_ERR_UNKNOWN_FIELD_ID      = 6,
        LOC_ERR_VALUE_UNAVAILIBLE     = 7,
        LOC_ERR_RECEIVER_BUSY         = 8,
        LOC_ERR_WAKE_UP               = 9,
        LOC_ERR_STAND_BY              = 10,

        LOC_ERR_INVALID
    }

    public enum LocData
    {
        LOC_DATA_DEVICE_INFO        = 0,
        LOC_DATA_MAX_REMOTE_TIMEOUT = 1,
        LOC_DATA_MAX_SUBSCRIBERS    = 2,
        LOC_DATA_PTS_PRESSURE       = 3,
        LOC_DATA_PTS_TEMPERATURE    = 4,
        LOC_DATA_PTS_DEPTH          = 5,
        LOC_DATA_CORE_TEMPERATURE   = 6,
        LOC_DATA_BAT_CHARGE         = 7,

        LOC_DATA_PRESSURE_RATING    = 8,
        LOC_DATA_ZERO_PRESSURE      = 9,
        LOC_DATA_WATER_DENSITY      = 10,
        LOC_DATA_SALINITY           = 11,
        LOC_DATA_SOUNDSPEED         = 12,
        LOC_DATA_GRAVITY_ACC        = 13,

        LOC_DATA_YEAR               = 14,
        LOC_DATA_MONTH              = 15,
        LOC_DATA_DATE               = 16,
        LOC_DATA_HOUR               = 17,
        LOC_DATA_MINUTE             = 18,
        LOC_DATA_SECOND             = 19,

        LOC_DATA_SUB_ID             = 20,

        LOC_DATA_UNKNOWN
    }

    public enum RemResult
    {
	    REM_RES_VALUE_NOT_SET = 1000,
	    REM_RES_NOT_SUPPORTED = 1001,
	    REM_RES_TIMEOUT       = 1002,
	    REM_RES_RESERVED_1    = 1003,
	    REM_RES_RESERVED_2    = 1004,
	    REM_RES_RESERVED_3    = 1005,
	    REM_RES_RESERVED_4    = 1006,
	    REM_RES_RESERVED_5    = 1007,
	    REM_RES_RESERVED_6    = 1008,
	    REM_RES_RESERVED_7    = 1009,
	    REM_RES_RESERVED_8    = 1010,
	    REM_RES_DEV_ALERT_1   = 1011,
	    REM_RES_DEV_ALERT_2   = 1012,
	    REM_RES_DEV_ALERT_3   = 1013,
	    REM_RES_DEV_ALERT_4   = 1014,
	    REM_RES_DEV_ALERT_5   = 1015,
	    REM_RES_DEV_ALERT_6   = 1016,
	    REM_RES_DEV_ALERT_7   = 1017,
	    REM_RES_DEV_ALERT_8   = 1018,
	    REM_RES_RESERVED_9    = 1019,
	    REM_RES_RESERVED_10   = 1020,
	    REM_RES_RESERVED_11   = 1021,
	    REM_RES_RESERVED_12   = 1022,

	    REM_RES_INVALID
    }

    public enum ServiceAct
    {
        LOC_INVOKE_FLASH_WRITE = 0,
        LOC_INVOKE_DPT_ZERO_ADJUST = 1,
        LOC_INVOKE_SYSTEM_RESET = 2,
        LOC_INVOKE_STAND_BY = 3,
        LOC_INVOKE_UNKNOWN
    }

    public enum CDS_CMD
    {
        CDS_CMD_PING = 0,
        CDS_CMD_PONG = 1,
        CDS_CMD_DPT = 2,
        CDS_CMD_TMP = 3,
        CDS_CMD_BAT = 4,
        CDS_CMD_USR_0 = 5,
        CDS_CMD_USR_1 = 6,
        CDS_CMD_USR_2 = 7,
        CDS_CMD_USR_3 = 8,
        CDS_CMD_USR_4 = 9,
        CDS_CMD_USR_5 = 10,
        CDS_CMD_USR_6 = 11,
        CDS_CMD_USR_7 = 12,
        CDS_CMD_USR_8 = 13,
        CDS_CMD_USR_9 = 14,
        CDS_CMD_USR_10 = 15,
        CDS_CMD_USR_11 = 16,
        CDS_CMD_USR_12 = 17,
        CDS_CMD_USR_13 = 18,
        CDS_CMD_USR_14 = 19,
        CDS_CMD_USR_15 = 20,
        CDS_CMD_USR_16 = 21,
        CDS_CMD_USR_17 = 22,
        CDS_CMD_USR_18 = 23,
        CDS_CMD_USR_19 = 24,
        CDS_CMD_USR_20 = 25,
        CDS_CMD_USR_21 = 26,
        CDS_CMD_USR_22 = 27,
        CDS_CMD_USR_23 = 28,
        CDS_CMD_USR_24 = 29,
        CDS_CMD_USR_25 = 30,
        CDS_CMD_USR_26 = 31,
        CDS_CMD_USR_27 = 32,
        CDS_CMD_USR_28 = 33,
        CDS_CMD_USR_29 = 34,
        CDS_CMD_USR_30 = 35,
        CDS_CMD_USR_31 = 36,
        CDS_CMD_USR_32 = 37,
        CDS_CMD_USR_33 = 38,
        CDS_CMD_USR_34 = 39,

        CDS_CMD_INVALID
    }

    public class GTR
    {
        public static readonly int MIN_REM_TOUT_MS = 2000;
    }
}
