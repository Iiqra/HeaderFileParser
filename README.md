# HeaderFileParser
This repository contains the code for .NET Core parser for C/C++ header file, can be extended to write code in other languages. 

Demo ↴↴↴↴

------------ CPP header file (File to be converted) ------------
```
/* sample.h  v2.8.3 */
#ifndef _WINDOWS_
#include <windows.h>
#endif

const int SAMPLE_CORE_SP = 6;	// SP_FLEX

#define ptGateway 'G' // application environment types
#define ptIDsNull -1 

// Structures for reference data calls

typedef struct {
	INT sampleIndex;
	CHAR sampleHistoric;
	CHAR sampleChecked;
	Array10 sampleOrderID;
	Array10 sampleDisplayID;
	Array30 sampleExchOrderID;
	Array10 sampleUserName;
	Array20 sampleTraderAccount;
	Array10 sampleOrderType;
	Array10 sampleExchangeName;
	Array10 sampleContractName;
	Array50 sampleContractDate;
	CHAR sampleBuyOrSell;
	Array20 samplePrice;
	Array20 samplePrice2;
	INT sampleLots;
	Array10 sampleLinkedOrder;
	INT sampleAmountFilled;
	INT sampleNoOfFills;
	Array20 sampleAveragePrice;
	BYTE sampleStatus;
	CHAR sampleOpenOrClose;
	Array8 sampleDateSent;
	Array6 sampleTimeSent;
	Array8 sampleDateHostRecd;
	Array6 sampleTimeHostRecd;
	Array8 sampleDateExchRecd;
	Array6 sampleTimeExchRecd;
	Array8 sampleDateExchAckn;
	Array6 sampleTimeExchAckn;
	Array60 sampleNonExecReason;
	INT sampleXref;
	INT sampleXrefP;
	INT sampleUpdateSeq;
	Array8 sampleGoodTillDate;
	Array25 sampleReference;
	INT samplePriority;
	Array8 sampleTriggerDate;
	Array6 sampleTriggerTime;
	INT sampleSubState;
	Array10 sampleBatchID;
	Array10 sampleBatchType;
	INT sampleBatchCount;
	Array10 sampleBatchStatus;
	Array10 sampleParentID;
	CHAR sampleDoneForDay;
	Array255 sampleBigRefField;
	INT sampleTimeout;
	Array120 sampleQuoteID;
	INT sampleLotsPosted;
	INT sampleChildCount;
	INT sampleActLots;
	Array32 sampleSenderLocationID;
	Array20 sampleRawprice;
	Array20 sampleRawprice2;
	Array70 sampleExecutionID;
	Array20 sampleClientID;
	Array50 sampleESARef;
	Array20 sampleISINCode;
	Array20 sampleCashPrice;
	CHAR sampleMethodology;
	Array20 sampleBasisRef;
	Array8 sampleEntryDate;
	Array6 sampleEntryTime;
	CHAR sampleAPIM;
	Array20 sampleAPIMUser;
	Array10 sampleICSNearLegPrice;
	Array10 sampleICSFarLegPrice;
	Array8 sampleCreationDate;
	INT sampleOrderHistorySeq;
	INT sampleMinClipSize;
	INT sampleMaxClipSize;
	CHAR sampleRandomise;
	CHAR sampleProfitLevel;
	INT sampleOFSeqNumber;
	Array10 sampleExchangeField;
	Array20 sampleBOFID;
	Array5 sampleBadge;
	INT sampleGTStatus;
	Array10 sampleLocalUserName;
	Array20 sampleLocalTrader;
	Array20 sampleLocalBOF;
	Array10 sampleLocalOrderID;
	Array10 sampleLocalExAcct;
	Array10 sampleRoutingID1;
	Array10 sampleRoutingID2;
	Array20 sampleFreeTextField1;
	Array20 sampleFreeTextField2;
	CHAR sampleInactive;
	Array20 sampleclientIdShortCode;
	CHAR sampleclientIdType;
	CHAR samplecommodityDerInd;
	CHAR sampleDEA;
	Array20 sampleexecutionDecision;
	CHAR sampleexecutionDecisionType;
	Array20 sampleinvestmentDecision;
	CHAR sampleinvestmentDecisionType;
	CHAR sampleliquidityProvider;
	CHAR sampleshortSelling;
	CHAR sampletradingCapacity;
	Array20 samplewaiverFlag;
	CHAR sampleancillaryTrading;
	Array20 samplerequestInTimeStamp;		
	Array20 samplerequestOutTimeStamp;
	Array20 sampleresponseInTimeStamp;
	Array20 sampleresponseOutTimeStamp;
	Array250 sampleextendedTxt;
} SampleOrderDetailStruct, FAR *SampeleOrderDetailStructPtr;


// Callback declarations
VOID WINAPI SampleHostStateChange(LinkStateStructPtr data);

#pragma pack (pop,SAMPLEAPI)

```
--------- PYTHON CTypes (Converted file)------------
```
SAMPLE_CORE_SP = 6
ptGateway = 'G'
ptIDsNull = -1

# Structures for reference data calls
class SampleOrderDetailStruct(Structure):
    _fields_ = [("sampleHistoric", c_char),
			 ("sampleChecked", c_char),
			 ("sampleOrderID", c_char*11),
			 ("sampleDisplayID", c_char*11),
			 ("sampleExchOrderID", c_char*31),
			 ("sampleUserName", c_char*11),
			 ("sampleTraderAccount", c_char*21),
			 ("sampleOrderType", c_char*11),
			 ("sampleExchangeName", c_char*11),
			 ("sampleContractName", c_char*11),
			 ("sampleContractDate", c_char*51),
			 ("sampleBuyOrSell", c_char),
			 ("samplePrice", c_char*21),
			 ("samplePrice2", c_char*21),
			 ("sampleLinkedOrder", c_char*11),
			 ("sampleAveragePrice", c_char*21),
			 ("sampleStatus", c_ubyte),
			 ("sampleOpenOrClose", c_char),
			 ("sampleDateSent", c_char*9),
			 ("sampleTimeSent", c_char*7),
			 ("sampleDateHostRecd", c_char*9),
			 ("sampleTimeHostRecd", c_char*7),
			 ("sampleDateExchRecd", c_char*9),
			 ("sampleTimeExchRecd", c_char*7),
			 ("sampleDateExchAckn", c_char*9),
			 ("sampleTimeExchAckn", c_char*7),
			 ("sampleNonExecReason", c_char*61),
			 ("sampleGoodTillDate", c_char*9),
			 ("sampleReference", c_char*26),
			 ("sampleTriggerDate", c_char*9),
			 ("sampleTriggerTime", c_char*7),
			 ("sampleBatchID", c_char*11),
			 ("sampleBatchType", c_char*11),
			 ("sampleBatchStatus", c_char*11),
			 ("sampleParentID", c_char*11),
			 ("sampleDoneForDay", c_char),
			 ("sampleBigRefField", c_char*256),
			 ("sampleQuoteID", c_char*121),
			 ("sampleSenderLocationID", c_char*33),
			 ("sampleRawprice", c_char*21),
			 ("sampleRawprice2", c_char*21),
			 ("sampleExecutionID", c_char*71),
			 ("sampleClientID", c_char*21),
			 ("sampleESARef", c_char*51),
			 ("sampleISINCode", c_char*21),
			 ("sampleCashPrice", c_char*21),
			 ("sampleMethodology", c_char),
			 ("sampleBasisRef", c_char*21),
			 ("sampleEntryDate", c_char*9),
			 ("sampleEntryTime", c_char*7),
			 ("sampleAPIM", c_char),
			 ("sampleAPIMUser", c_char*21),
			 ("sampleICSNearLegPrice", c_char*11),
			 ("sampleICSFarLegPrice", c_char*11),
			 ("sampleCreationDate", c_char*9),
			 ("sampleRandomise", c_char),
			 ("sampleProfitLevel", c_char),
			 ("sampleExchangeField", c_char*11),
			 ("sampleBOFID", c_char*21),
			 ("sampleBadge", c_char*6),
			 ("sampleLocalUserName", c_char*11),
			 ("sampleLocalTrader", c_char*21),
			 ("sampleLocalBOF", c_char*21),
			 ("sampleLocalOrderID", c_char*11),
			 ("sampleLocalExAcct", c_char*11),
			 ("sampleRoutingID1", c_char*11),
			 ("sampleRoutingID2", c_char*11),
			 ("sampleFreeTextField1", c_char*21),
			 ("sampleFreeTextField2", c_char*21),
			 ("sampleInactive", c_char),
			 ("sampleclientIdShortCode", c_char*21),
			 ("sampleclientIdType", c_char),
			 ("samplecommodityDerInd", c_char),
			 ("sampleDEA", c_char),
			 ("sampleexecutionDecision", c_char*21),
			 ("sampleexecutionDecisionType", c_char),
			 ("sampleinvestmentDecision", c_char*21),
			 ("sampleinvestmentDecisionType", c_char),
			 ("sampleliquidityProvider", c_char),
			 ("sampleshortSelling", c_char),
			 ("sampletradingCapacity", c_char),
			 ("samplewaiverFlag", c_char*21),
			 ("sampleancillaryTrading", c_char),
			 ("samplerequestInTimeStamp", c_char*21),
			 ("samplerequestOutTimeStamp", c_char*21),
			 ("sampleresponseInTimeStamp", c_char*21),
			 ("sampleresponseOutTimeStamp", c_char*21),
			 ("sampleextendedTxt", c_char*251)]

# Callback declarations
SampleHostStateChange = CFUNCTYPE(None, POINTER(LinkStateStruct))
```



















