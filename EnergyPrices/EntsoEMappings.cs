using Microsoft.AspNetCore.Authorization;

public class EntsoEArea
{
    public EntsoEArea( string name, string code, string description, string timeZone)
    {
        Name = name;
        Code = code;
        Description = description;
        TimeZone = timeZone;
    }

    public string Name {get;set;}
    public string Code {get;set;}
    public string Description {get;set;}
    public string TimeZone {get;set;}
}

public class EntosEAreas: List<EntsoEArea>
{
    public EntosEAreas()
    {
        EntsoEArea[] areas = { 
            // List taken directly from the API Docs
            new EntsoEArea("DE_50HZ",      "10YDE-VE-------2","50Hertz CA, DE(50HzT) BZA",                   "Europe/Berlin"),
            new EntsoEArea("DE_50HZ",      "10YDE-VE-------2","50Hertz CA, DE(50HzT) BZA",                   "Europe/Berlin"),
            new EntsoEArea("AL",           "10YAL-KESH-----5","Albania, OST BZ / CA / MBA",                  "Europe/Tirane"),
            new EntsoEArea("DE_AMPRION",   "10YDE-RWENET---I","Amprion CA",                                  "Europe/Berlin"),
            new EntsoEArea("AT",           "10YAT-APG------L","Austria, APG BZ / CA / MBA",                  "Europe/Vienna"),
            new EntsoEArea("BY",           "10Y1001A1001A51S","Belarus BZ / CA / MBA",                       "Europe/Minsk"),
            new EntsoEArea("BE",           "10YBE----------2","Belgium, Elia BZ / CA / MBA",                 "Europe/Brussels"),
            new EntsoEArea("BA",           "10YBA-JPCC-----D","Bosnia Herzegovina, NOS BiH BZ / CA / MBA",   "Europe/Sarajevo"),
            new EntsoEArea("BG",           "10YCA-BULGARIA-R","Bulgaria, ESO BZ / CA / MBA",                 "Europe/Sofia"),
            new EntsoEArea("CZ_DE_SK",     "10YDOM-CZ-DE-SKK","BZ CZ+DE+SK BZ / BZA",                        "Europe/Prague"),
            new EntsoEArea("HR",           "10YHR-HEP------M","Croatia, HOPS BZ / CA / MBA",                 "Europe/Zagreb"),
            new EntsoEArea("CWE",          "10YDOM-REGION-1V","CWE Region",                                  "Europe/Brussels"),
            new EntsoEArea("CY",           "10YCY-1001A0003J","Cyprus, Cyprus TSO BZ / CA / MBA",            "Asia/Nicosia"),
            new EntsoEArea("CZ",           "10YCZ-CEPS-----N","Czech Republic, CEPS BZ / CA/ MBA",           "Europe/Prague"),
            new EntsoEArea("DE_AT_LU",     "10Y1001A1001A63L","DE-AT-LU BZ",                                 "Europe/Berlin"),
            new EntsoEArea("DE_LU",        "10Y1001A1001A82H","DE-LU BZ / MBA",                              "Europe/Berlin"),
            new EntsoEArea("DK",           "10Y1001A1001A65H","Denmark",                                     "Europe/Copenhagen"),
            new EntsoEArea("DK_1",         "10YDK-1--------W","DK1 BZ / MBA",                                "Europe/Copenhagen"),
            new EntsoEArea("DK_1_NO_1",    "46Y000000000007M","DK1 NO1 BZ",                                  "Europe/Copenhagen"),
            new EntsoEArea("DK_2",         "10YDK-2--------M","DK2 BZ / MBA",                                "Europe/Copenhagen"),
            new EntsoEArea("DK_CA",        "10Y1001A1001A796","Denmark, Energinet CA",                       "Europe/Copenhagen"),
            new EntsoEArea("EE",           "10Y1001A1001A39I","Estonia, Elering BZ / CA / MBA",              "Europe/Tallinn"),
            new EntsoEArea("FI",           "10YFI-1--------U","Finland, Fingrid BZ / CA / MBA",              "Europe/Helsinki"),
            new EntsoEArea("MK",           "10YMK-MEPSO----8","Former Yugoslav Republic of Macedonia, MEPSO BZ / CA / MBA","Europe/Skopje"),
            new EntsoEArea("FR",           "10YFR-RTE------C","France, RTE BZ / CA / MBA",                   "Europe/Paris"),
            new EntsoEArea("DE",           "10Y1001A1001A83F","Germany",                                     "Europe/Berlin"),
            new EntsoEArea("GR",           "10YGR-HTSO-----Y","Greece, IPTO BZ / CA/ MBA",                   "Europe/Athens"),
            new EntsoEArea("HU",           "10YHU-MAVIR----U","Hungary, MAVIR CA / BZ / MBA",                "Europe/Budapest"),
            new EntsoEArea("IS",           "IS",              "Iceland",                                     "Atlantic/Reykjavik"),
            new EntsoEArea("IE_SEM",       "10Y1001A1001A59C","Ireland (SEM) BZ / MBA",                      "Europe/Dublin"),
            new EntsoEArea("IE",           "10YIE-1001A00010","Ireland, EirGrid CA",                         "Europe/Dublin"),
            new EntsoEArea("IT",           "10YIT-GRTN-----B","Italy, IT CA / MBA",                          "Europe/Rome"),
            new EntsoEArea("IT_SACO_AC",   "10Y1001A1001A885","Italy_Saco_AC",                               "Europe/Rome"),
            new EntsoEArea("IT_CALA",  "10Y1001C--00096J","IT-Calabria BZ",                               "Europe/Rome"),
            new EntsoEArea("IT_SACO_DC",   "10Y1001A1001A893","Italy_Saco_DC",                               "Europe/Rome"),
            new EntsoEArea("IT_BRNN",      "10Y1001A1001A699","IT-Brindisi BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_CNOR",      "10Y1001A1001A70O","IT-Centre-North BZ",                          "Europe/Rome"),
            new EntsoEArea("IT_CSUD",      "10Y1001A1001A71M","IT-Centre-South BZ",                          "Europe/Rome"),
            new EntsoEArea("IT_FOGN",      "10Y1001A1001A72K","IT-Foggia BZ",                                "Europe/Rome"),
            new EntsoEArea("IT_GR",        "10Y1001A1001A66F","IT-GR BZ",                                    "Europe/Rome"),
            new EntsoEArea("IT_MACRO_NORTH","10Y1001A1001A84D","IT-MACROZONE NORTH MBA",                     "Europe/Rome"),
            new EntsoEArea("IT_MACRO_SOUTH","10Y1001A1001A85B","IT-MACROZONE SOUTH MBA",                     "Europe/Rome"),
            new EntsoEArea("IT_MALTA",     "10Y1001A1001A877","IT-Malta BZ",                                 "Europe/Rome"),
            new EntsoEArea("IT_NORD",      "10Y1001A1001A73I","IT-North BZ",                                 "Europe/Rome"),
            new EntsoEArea("IT_NORD_AT",   "10Y1001A1001A80L","IT-North-AT BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_NORD_CH",   "10Y1001A1001A68B","IT-North-CH BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_NORD_FR",   "10Y1001A1001A81J","IT-North-FR BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_NORD_SI",   "10Y1001A1001A67D","IT-North-SI BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_PRGP",      "10Y1001A1001A76C","IT-Priolo BZ",                                "Europe/Rome"),
            new EntsoEArea("IT_ROSN",      "10Y1001A1001A77A","IT-Rossano BZ",                               "Europe/Rome"),
            new EntsoEArea("IT_SARD",      "10Y1001A1001A74G","IT-Sardinia BZ",                              "Europe/Rome"),
            new EntsoEArea("IT_SICI",      "10Y1001A1001A75E","IT-Sicily BZ",                                "Europe/Rome"),
            new EntsoEArea("IT_SUD",       "10Y1001A1001A788","IT-South BZ",                                 "Europe/Rome"),
            new EntsoEArea("RU_KGD",       "10Y1001A1001A50U","Kaliningrad BZ / CA / MBA",                   "Europe/Kaliningrad"),
            new EntsoEArea("LV",           "10YLV-1001A00074","Latvia, AST BZ / CA / MBA",                   "Europe/Riga"),
            new EntsoEArea("LT",           "10YLT-1001A0008Q","Lithuania, Litgrid BZ / CA / MBA",            "Europe/Vilnius"),
            new EntsoEArea("LU",           "10YLU-CEGEDEL-NQ","Luxembourg, CREOS CA",                        "Europe/Luxembourg"),
            new EntsoEArea("LU_BZN",       "10Y1001A1001A82H","Luxembourg",                                  "Europe/Luxembourg"),
            new EntsoEArea("MT",           "10Y1001A1001A93C","Malta, Malta BZ / CA / MBA",                  "Europe/Malta"),
            new EntsoEArea("ME",           "10YCS-CG-TSO---S","Montenegro, CGES BZ / CA / MBA",              "Europe/Podgorica"),
            new EntsoEArea("GB",           "10YGB----------A","National Grid BZ / CA/ MBA",                  "Europe/London"),
            new EntsoEArea("GB_IFA",       "10Y1001C--00098F","GB(IFA) BZN",                                 "Europe/London"),
            new EntsoEArea("GB_IFA2",      "17Y0000009369493","GB(IFA2) BZ",                                 "Europe/London"),
            new EntsoEArea("GB_ELECLINK",  "11Y0-0000-0265-K","GB(ElecLink) BZN",                            "Europe/London"),
            new EntsoEArea("UK",           "10Y1001A1001A92E","United Kingdom",                              "Europe/London"),
            new EntsoEArea("NL",           "10YNL----------L","Netherlands, TenneT NL BZ / CA/ MBA",         "Europe/Amsterdam"),
            new EntsoEArea("NO_1",         "10YNO-1--------2","NO1 BZ / MBA",                                "Europe/Oslo"),
            new EntsoEArea("NO_1A",        "10Y1001A1001A64J","NO1 A BZ",                                    "Europe/Oslo"),
            new EntsoEArea("NO_2",         "10YNO-2--------T","NO2 BZ / MBA",                                "Europe/Oslo"),
            new EntsoEArea("NO_2_NSL",     "50Y0JVU59B4JWQCU","NO2 NSL BZ / MBA",                            "Europe/Oslo"),
            new EntsoEArea("NO_2A",        "10Y1001C--001219","NO2 A BZ",                                    "Europe/Oslo"),
            new EntsoEArea("NO_3",         "10YNO-3--------J","NO3 BZ / MBA",                                "Europe/Oslo"),
            new EntsoEArea("NO_4",         "10YNO-4--------9","NO4 BZ / MBA",                                "Europe/Oslo"),
            new EntsoEArea("NO_5",         "10Y1001A1001A48H","NO5 BZ / MBA",                                "Europe/Oslo"),
            new EntsoEArea("NO",           "10YNO-0--------C","Norway, Norway MBA, Stattnet CA",             "Europe/Oslo"),
            new EntsoEArea("PL_CZ",        "10YDOM-1001A082L","PL-CZ BZA / CA",                              "Europe/Warsaw"),
            new EntsoEArea("PL",           "10YPL-AREA-----S","Poland, PSE SA BZ / BZA / CA / MBA",          "Europe/Warsaw"),
            new EntsoEArea("PT",           "10YPT-REN------W","Portugal, REN BZ / CA / MBA",                 "Europe/Lisbon"),
            new EntsoEArea("MD",           "10Y1001A1001A990","Republic of Moldova, Moldelectica BZ/CA/MBA", "Europe/Chisinau"),
            new EntsoEArea("RO",           "10YRO-TEL------P","Romania, Transelectrica BZ / CA/ MBA",        "Europe/Bucharest"),
            new EntsoEArea("RU",           "10Y1001A1001A49F","Russia BZ / CA / MBA",                        "Europe/Moscow"),
            new EntsoEArea("SE_1",         "10Y1001A1001A44P","SE1 BZ / MBA",                                "Europe/Stockholm"),
            new EntsoEArea("SE_2",         "10Y1001A1001A45N","SE2 BZ / MBA",                                "Europe/Stockholm"),
            new EntsoEArea("SE_3",         "10Y1001A1001A46L","SE3 BZ / MBA",                                "Europe/Stockholm"),
            new EntsoEArea("SE_4",         "10Y1001A1001A47J","SE4 BZ / MBA",                                "Europe/Stockholm"),
            new EntsoEArea("RS",           "10YCS-SERBIATSOV","Serbia, EMS BZ / CA / MBA",                   "Europe/Belgrade"),
            new EntsoEArea("SK",           "10YSK-SEPS-----K","Slovakia, SEPS BZ / CA / MBA",                "Europe/Bratislava"),
            new EntsoEArea("SI",           "10YSI-ELES-----O","Slovenia, ELES BZ / CA / MBA",                "Europe/Ljubljana"),
            new EntsoEArea("GB_NIR",       "10Y1001A1001A016","Northern Ireland, SONI CA",                   "Europe/Belfast"),
            new EntsoEArea("ES",           "10YES-REE------0","Spain, REE BZ / CA / MBA",                    "Europe/Madrid"),
            new EntsoEArea("SE",           "10YSE-1--------K","Sweden, Sweden MBA, SvK CA",                  "Europe/Stockholm"),
            new EntsoEArea("CH",           "10YCH-SWISSGRIDZ","Switzerland, Swissgrid BZ / CA / MBA",        "Europe/Zurich"),
            new EntsoEArea("DE_TENNET",    "10YDE-EON------1","TenneT GER CA",                               "Europe/Berlin"),
            new EntsoEArea("DE_TRANSNET",  "10YDE-ENBW-----N","TransnetBW CA",                               "Europe/Berlin"),
            new EntsoEArea("TR",           "10YTR-TEIAS----W","Turkey BZ / CA / MBA",                        "Europe/Istanbul"),
            new EntsoEArea("UA",           "10Y1001C--00003F","Ukraine, Ukraine BZ, MBA",                    "Europe/Kiev"),
            new EntsoEArea("UA_DOBTPP",    "10Y1001A1001A869","Ukraine-DobTPP CTA",                          "Europe/Kiev"),
            new EntsoEArea("UA_BEI",       "10YUA-WEPS-----0","Ukraine BEI CTA",                             "Europe/Kiev"),
            new EntsoEArea("UA_IPS",       "10Y1001C--000182","Ukraine IPS CTA",                             "Europe/Kiev"),
            new EntsoEArea("XK",           "10Y1001C--00100H","Kosovo/ XK CA / XK BZN",                      "Europe/Rome"),
            new EntsoEArea("DE_AMP_LU",    "10Y1001C--00002H","Amprion LU CA",                               "Europe/Berlin")
                };
    this.AddRange( areas.AsEnumerable<EntsoEArea>());
    }
}

