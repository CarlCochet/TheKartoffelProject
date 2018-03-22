using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Formulas
{
    public static class AdditionalFormulas
    {
        public const int STATS_ADD_PM2 = 78;
        public const int STATS_REM_PA = 101;
        public const int STATS_ADD_VIE = 110;
        public const int STATS_ADD_PA = 111;
        public const int STATS_MULTIPLY_DOMMAGE = 114;
        public const int STATS_ADD_CC = 115;
        public const int STATS_REM_PO = 116;
        public const int STATS_ADD_PO = 117;
        public const int STATS_ADD_FORC = 118;
        public const int STATS_ADD_AGIL = 119;
        public const int STATS_ADD_PA2 = 120;
        public const int STATS_ADD_DOMA = 121;
        public const int STATS_ADD_EC = 122;
        public const int STATS_ADD_CHAN = 123;
        public const int STATS_ADD_SAGE = 124;
        public const int STATS_ADD_VITA = 125;
        public const int STATS_ADD_INTE = 126;
        public const int STATS_REM_PM = 127;
        public const int STATS_ADD_PM = 128;
        public const int STATS_ADD_PERDOM = 138;
        public const int STATS_ADD_PDOM = 142;
        public const int STATS_REM_DOMA = 145;
        public const int STATS_REM_CHAN = 152;
        public const int STATS_REM_VITA = 153;
        public const int STATS_REM_AGIL = 154;
        public const int STATS_REM_INTE = 155;
        public const int STATS_REM_SAGE = 156;
        public const int STATS_REM_FORC = 157;
        public const int STATS_ADD_PODS = 158;
        public const int STATS_REM_PODS = 159;
        public const int STATS_ADD_AFLEE = 160;
        public const int STATS_ADD_MFLEE = 161;
        public const int STATS_REM_AFLEE = 162;
        public const int STATS_REM_MFLEE = 163;
        public const int STATS_ADD_MAITRISE = 165;
        public const int STATS_REM_PA2 = 168;
        public const int STATS_REM_PM2 = 169;
        public const int STATS_REM_CC = 171;
        public const int STATS_ADD_INIT = 174;
        public const int STATS_REM_INIT = 175;
        public const int STATS_ADD_PROS = 176;
        public const int STATS_REM_PROS = 177;
        public const int STATS_ADD_SOIN = 178;
        public const int STATS_REM_SOIN = 179;
        public const int STATS_CREATURE = 182;
        public const int STATS_ADD_RP_TER = 210;
        public const int STATS_ADD_RP_EAU = 211;
        public const int STATS_ADD_RP_AIR = 212;
        public const int STATS_ADD_RP_FEU = 213;
        public const int STATS_ADD_RP_NEU = 214;
        public const int STATS_REM_RP_TER = 215;
        public const int STATS_REM_RP_EAU = 216;
        public const int STATS_REM_RP_AIR = 217;
        public const int STATS_REM_RP_FEU = 218;
        public const int STATS_REM_RP_NEU = 219;
        public const int STATS_RETDOM = 220;
        public const int STATS_TRAPDOM = 225;
        public const int STATS_TRAPPER = 226;
        public const int STATS_ADD_R_FEU = 240;
        public const int STATS_ADD_R_NEU = 241;
        public const int STATS_ADD_R_TER = 242;
        public const int STATS_ADD_R_EAU = 243;
        public const int STATS_ADD_R_AIR = 244;
        public const int STATS_REM_R_FEU = 245;
        public const int STATS_REM_R_NEU = 246;
        public const int STATS_REM_R_TER = 247;
        public const int STATS_REM_R_EAU = 248;
        public const int STATS_REM_R_AIR = 249;
        public const int STATS_ADD_RP_PVP_TER = 250;
        public const int STATS_ADD_RP_PVP_EAU = 251;
        public const int STATS_ADD_RP_PVP_AIR = 252;
        public const int STATS_ADD_RP_PVP_FEU = 253;
        public const int STATS_ADD_RP_PVP_NEU = 254;
        public const int STATS_REM_RP_PVP_TER = 255;
        public const int STATS_REM_RP_PVP_EAU = 256;
        public const int STATS_REM_RP_PVP_AIR = 257;
        public const int STATS_REM_RP_PVP_FEU = 258;
        public const int STATS_REM_RP_PVP_NEU = 259;
        public const int STATS_ADD_R_PVP_TER = 260;
        public const int STATS_ADD_R_PVP_EAU = 261;
        public const int STATS_ADD_R_PVP_AIR = 262;
        public const int STATS_ADD_R_PVP_FEU = 263;
        public const int STATS_ADD_R_PVP_NEU = 264;
        public static int[] ID_EFECTOS_ARMAS = { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 108 };
        public static double getPwrPerEffet(int effect)
        {
            var r = 0.0;
            switch (effect)
            {
                case 125:
                    return 0.25;
                case 158:
                    return 0.25;
                case 174:
                    return 0.25;
                case 118:
                    return 1;
                case 126:
                    return 1;
                case 119:
                    return 1;
                case 123:
                    return 1;
                case 138:
                    return 2;
                case 226:
                    return 2;
                case 243:
                    return 2;
                case 242:
                    return 2;
                case 241:
                    return 2;
                case 240:
                    return 2;
                case 244:
                    return 2;
                case 416:
                    return 2;
                case 420:
                    return 2;
                case 124:
                    return 3;
                case 176:
                    return 3;
                case 752:
                    return 4;
                case 753:
                    return 4;
                case 795:
                    return 5;
                case 414:
                    return 5;
                case 418:
                    return 5;
                case 422:
                    return 5;
                case 424:
                    return 5;
                case 426:
                    return 5;
                case 428:
                    return 5;
                case 430:
                    return 5;
                case 213:
                    return 6;
                case 212:
                    return 6;
                case 210:
                    return 6;
                case 214:
                    return 6;
                case 211:
                    return 6;
                case 160:
                    return 7;
                case 161:
                    return 7;
                case 410:
                    return 7;
                case 412:
                    return 7;
                case 115:
                    return 10;
                case 178:
                    return 10;
                case 225:
                    return 15;
                case 112:
                    return 20;
                case 220:
                    return 30;
                case 182:
                    return 30;
                case 117:
                    return 51;
                case 128:
                    return 90;
                case 111:
                    return 100;
                default:
                    if (!ID_EFECTOS_ARMAS.Contains(effect))
                        Console.WriteLine($"Not handled effect {effect}");
                    break;
            }
            return r;
        }

        public static double getOverPerEffet(int effect)
        {
            var r = 0.0;
            switch (effect)
            {
                case STATS_ADD_PA:
                    r = 0.0;
                    break;

                case STATS_ADD_PM2:
                    r = 404.0;
                    break;

                case STATS_ADD_VIE:
                    r = 404.0;
                    break;

                case STATS_MULTIPLY_DOMMAGE:
                    r = 0.0;
                    break;

                case STATS_ADD_CC:
                    r = 3.0;
                    break;

                case STATS_ADD_PO:
                    r = 0.0;
                    break;

                case STATS_ADD_FORC:
                    r = 101.0;
                    break;

                case STATS_ADD_AGIL:
                    r = 101.0;
                    break;

                case STATS_ADD_PA2:
                    r = 0.0;
                    break;

                case STATS_ADD_DOMA:
                    r = 5.0;
                    break;

                case STATS_ADD_EC:
                    r = 0.0;
                    break;

                case STATS_ADD_CHAN:
                    r = 101.0;
                    break;

                case STATS_ADD_SAGE:
                    r = 33.0;
                    break;

                case STATS_ADD_VITA:
                    r = 404.0;
                    break;

                case STATS_ADD_INTE:
                    r = 101.0;
                    break;

                case STATS_ADD_PM:
                    r = 0.0;
                    break;

                case STATS_ADD_PERDOM:
                    r = 50.0;
                    break;

                case STATS_ADD_PDOM:
                    r = 50.0;
                    break;

                case STATS_ADD_PODS:
                    r = 404.0;
                    break;

                case STATS_ADD_AFLEE:
                    r = 0.0;
                    break;

                case STATS_ADD_MFLEE:
                    r = 0.0;
                    break;

                case STATS_ADD_INIT:
                    r = 1010.0;
                    break;

                case STATS_ADD_PROS:
                    r = 33.0;
                    break;

                case STATS_ADD_SOIN:
                    r = 5.0;
                    break;

                case STATS_CREATURE:
                    r = 3.0;
                    break;

                case STATS_ADD_RP_TER:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_EAU:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_AIR:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_FEU:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_NEU:
                    r = 16.0;
                    break;

                case STATS_TRAPDOM:
                    r = 6.0;
                    break;

                case STATS_TRAPPER:
                    r = 50.0;
                    break;

                case STATS_ADD_R_FEU:
                    r = 50.0;
                    break;

                case STATS_ADD_R_NEU:
                    r = 50.0;
                    break;

                case STATS_ADD_R_TER:
                    r = 50.0;
                    break;

                case STATS_ADD_R_EAU:
                    r = 50.0;
                    break;

                case STATS_ADD_R_AIR:
                    r = 50.0;
                    break;

                case STATS_ADD_RP_PVP_TER:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_PVP_EAU:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_PVP_AIR:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_PVP_FEU:
                    r = 16.0;
                    break;

                case STATS_ADD_RP_PVP_NEU:
                    r = 16.0;
                    break;

                case STATS_ADD_R_PVP_TER:
                    r = 50.0;
                    break;

                case STATS_ADD_R_PVP_EAU:
                    r = 50.0;
                    break;

                case STATS_ADD_R_PVP_AIR:
                    r = 50.0;
                    break;

                case STATS_ADD_R_PVP_FEU:
                    r = 50.0;
                    break;

                case STATS_ADD_R_PVP_NEU:
                    r = 50.0;
                    break;
            }
            return r;
        }

        public static int GetMulti(int statID)
        {
            var multi = 0;
            switch (statID)
            {
                case 125:
                case 158:
                case 174:
                    return 1;//this are 0.25
                case 118:
                case 126:
                case 119:
                case 123:
                    return 1;
                case 138:
                case 226:
                case 243:
                case 242:
                case 241:
                case 240:
                case 244:
                case 416:
                case 420:
                    return 2;
                case 124:
                case 176:
                    return 3;
                case 752:
                case 753:
                    return 4;
                case 795:
                case 414:
                case 418:
                case 422:
                case 424:
                case 426:
                case 428:
                case 430:
                    return 5;
                case 213:
                case 212:
                case 210:
                case 214:
                case 211:
                    return 6;
                case 160:
                case 161:
                case 410:
                case 412:
                    return 7;
                case 115:
                case 178:
                    return 10;
                case 225:
                    return 15;
                case 112:
                    return 20;
                case 220:
                case 182:
                    return 30;
                case 117:
                    return 51;
                case 128:
                    return 90;
                case 111:
                    return 100;
                default:
                    Console.WriteLine($"Fatal error no managed effect! | EffectId : {statID}");
                    break;
            }
            return multi;
        }

        public static double GetRunePower(int templateId)
        {
            switch (templateId)
            {
                //potions
                case 1333:
                case 1335:
                case 1337:
                case 1338:
                    return 50;

                case 1340:
                case 1341:
                case 1342:
                case 1343:
                    return 68;

                case 1345:
                case 1346:
                case 1347:
                case 1348:
                    return 85;
                //rune
                case 1557:
                    return 100;

                case 1558:
                    return 90;

                case 7438:
                    return 51;

                case 7442:
                case 7437:
                    return 30;

                case 7435:
                    return 20;

                case 7446:
                case 10613:
                    return 15;

                case 7433:
                case 7434:
                    return 10;

                case 11642:
                case 11644:
                case 11646:
                case 11648:
                case 11641:
                case 11643:
                case 11645:
                case 11647:
                    return 7;

                case 7457:
                case 7458:
                case 7459:
                case 7560:
                case 7460:
                    return 6;

                case 10057:
                case 11664:
                case 11658:
                case 11666:
                case 11650:
                case 11660:
                case 11654:
                case 11662:
                case 11649:
                case 11663:
                case 11657:
                case 11665:
                case 11659:
                case 11653:
                case 11661:
                    return 5;

                case 11640:
                case 11639:
                case 11638:
                case 11637:
                    return 4;

                case 1552:
                case 1546:
                case 1521:
                case 10662:
                case 7451:
                    return 3;

                case 10619:
                case 10618:
                case 7436:
                case 10616:
                case 10615:
                case 7447:
                case 11656:
                case 11652:
                case 7455:
                case 7456:
                case 11651:
                case 7452:
                case 7453:
                case 11655:
                case 7454:
                    return 2;

                case 1551:
                case 1545:
                case 1519:
                case 1553:
                case 1547:
                case 1522:
                case 1555:
                case 1549:
                case 1524:
                case 1556:
                case 1550:
                case 1525:
                    return 1;

                case 7445:
                case 7444:
                case 7443:
                case 1554:
                case 1548:
                case 1523:
                case 7450:
                case 7449:
                case 7448:
                    return 0.25;
            }
            return -1;
        }

        public static int StatRune(int stat, ref short cant)
        {
            switch (stat)
            {
                case 111:
                    return 1557;

                case 112:
                    return 7435;

                case 115:
                    return 7433;

                case 117:
                    return 7438;

                case 118:
                    if (cant > 70)
                    {
                        cant /= 10;
                        return 1551;
                    }

                    if (cant <= 70 && cant > 20)
                    {
                        cant /= 3;
                        return 1545;
                    }

                    return 1519;

                case 119:
                    if (cant > 70)
                    {
                        cant /= 10;
                        return 1555;
                    }

                    if (cant <= 70 && cant > 20)
                    {
                        cant /= 3;
                        return 1549;
                    }

                    return 1524;

                case 123:
                    if (cant > 70)
                    {
                        cant /= 10;
                        return 1556;
                    }

                    if (cant <= 70 && cant > 20)
                    {
                        cant /= 3;
                        return 1550;
                    }

                    return 1525;

                case 124:
                    if (cant > 30)
                    {
                        cant /= 10;
                        return 1552;
                    }

                    if (cant <= 30 && cant > 10)
                    {
                        cant /= 3;
                        return 1546;
                    }

                    return 1521;

                case 125:
                    if (cant > 230)
                    {
                        cant /= 50;
                        return 1554;
                    }

                    if (cant <= 230 && cant > 60)
                    {
                        cant /= 15;
                        return 1548;
                    }

                    cant /= 5;
                    return 1523;

                case 126:
                    if (cant > 70)
                    {
                        cant /= 10;
                        return 1553;
                    }

                    if (cant <= 70 && cant > 20)
                    {
                        cant /= 3;
                        return 1547;
                    }

                    return 1522;

                case 128:
                    return 1558;

                case 138:
                    return 7436;

                case 158:
                    if (cant > 300)
                    {
                        cant /= 100;
                        return 7445;
                    }

                    if (cant <= 300 && cant > 100)
                    {
                        cant /= 30;
                        return 7444;
                    }

                    cant /= 10;
                    return 7443;

                case 174:
                    if (cant > 300)
                    {
                        cant /= 100;
                        return 7450;
                    }

                    if (cant <= 300 && cant > 100)
                    {
                        cant /= 30;
                        return 7449;
                    }

                    cant /= 10;
                    return 7448;

                case 176:
                    if (cant > 5)
                    {
                        cant /= 3;
                        return 10662;
                    }

                    return 7451;

                case 178:
                    return 7434;

                case 182:
                    return 7442;

                case 220:
                    return 7437;

                case 225:
                    return 7446;

                case 226:
                    return 7447;

                case 240:
                    return 7452;

                case 241:
                    return 7453;

                case 242:
                    return 7454;

                case 243:
                    return 7455;

                case 244:
                    return 7456;

                case 210:
                    return 7457;

                case 211:
                    return 7458;

                case 212:
                    return 7560;

                case 213:
                    return 7459;

                case 214:
                    return 7460;

                default:
                    return 0;

            }
        }
    }

}
