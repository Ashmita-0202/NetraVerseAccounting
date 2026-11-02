using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Static.Cache
{
    public static class CacheKeys
    {

        #region SetupModules
        public const string Module = "Module";
        public const string Company = "CompanyCacheKey";
        public const string District = "DistrictCacheKey";
        public const string Institution = "InstitutionCachekey";
        public const string Menu = "MenuCacheKey";
        public const string Municipality = "MunicipalityCacheKey";
        public const string Organization = "OrganizationCacheKey";
        public const string Province = "ProvinceCacheKey";
        public const string SubModules = "SubModulesCacheKey";
        public const string Vdc = "VdcCacheKey";
        #endregion

        #region AccountModules
        public const string LedgerGroup = "LedgerGroupCacheKey";
        public const string Ledger = "LedgerCacheKey";
        public const string Master = "MasterCacheKey";


        #endregion
    }
}
