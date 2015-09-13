using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = MLM.Models;
using ViewModels = MyLegacyMaps.Models;

namespace MyLegacyMaps.Extensions
{
    public static class ViewModelConvertor
    {
        public static ViewModels.AdoptedMap ToViewModel(this DomainModel.AdoptedMap value)
        {
            if (value == null)
                return null;

            return new ViewModels.AdoptedMap
            {
                AdoptedMapId = value.AdoptedMapId,
                MapId = value.MapId,
                Name = value.Name,
                Description = value.Description,
                IsActive = value.IsActive,
                UserId = value.UserId,
                ShareStatusTypeId = value.ShareStatusTypeId,
                DateCreated = value.DateCreated,
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy,
                Map = value.Map.ToViewModel(),
                ShareStatusType = value.ShareStatusType.ToViewModel(),
                Flags = value.Flags.ToViewModel()
            };
        }

        public static List<ViewModels.AdoptedMap> ToViewModel(this ICollection<DomainModel.AdoptedMap> value)
        {
            if (value == null)
                return null;

            List<ViewModels.AdoptedMap> retVal = new List<ViewModels.AdoptedMap>();
            foreach (var adoptedMap in value)
            {
                retVal.Add(adoptedMap.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.Map ToViewModel(this DomainModel.Map value)
        {
            if (value == null)
                return null;

            return new ViewModels.Map
            {
                MapId = value.MapId,
                AspectRatioId = value.AspectRatioId,
                Name = value.Name,
                Description = value.Description,
                ImageUrl = value.ImageUrl,
                ThumbUrl = value.ThumbUrl,
                FileName = value.FileName,
                IsActive = value.IsActive,                
                OrientationTypeId = value.OrientationTypeId,
                DateCreated = value.DateCreated,
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy,
                MapTypes = value.MapTypes.ToViewModel(true),
                OrientationType = value.OrientationType.ToViewModel(),
                AspectRatio = value.AspectRatio.ToViewModel()
            };
        }

        public static List<ViewModels.Map> ToViewModel(this ICollection<DomainModel.Map> value, bool includeRealEstate = false)
        {
            if (value == null)
                return null;

            var realEstateMapType = new DomainModel.MapType { MapTypeId = 1, Name = "Real Estate", IsActive = true };
            List<ViewModels.Map> retVal = new List<ViewModels.Map>();
            foreach (var map in value)
            {
                if(includeRealEstate == false && map.MapTypes.Contains(realEstateMapType))
                {
                    continue;
                }
                retVal.Add(map.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.MapType ToViewModel(this DomainModel.MapType value)
        {
            if (value == null)
                return null;

            return new ViewModels.MapType
            {
                MapTypeId = value.MapTypeId,
                Name = value.Name,
                IsActive = value.IsActive
            };
        }

        public static List<ViewModels.MapType> ToViewModel(this ICollection<DomainModel.MapType> value, bool includeRealEstate)
        {
            List<ViewModels.MapType> retVal = new List<ViewModels.MapType>();
            foreach (var type in value)
            {
                if (type.MapTypeId == 1 && !includeRealEstate) //Hide Real Estate Map Type from View
                    continue;

                retVal.Add(type.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.ShareStatusType ToViewModel(this DomainModel.ShareStatusType value)
        {
            if (value == null)
                return null;

            return new ViewModels.ShareStatusType
            {
                ShareStatusTypeId = value.ShareStatusTypeId,
                Name = value.Name
            };
        }

        public static ICollection<ViewModels.ShareStatusType> ToViewModel(this ICollection<DomainModel.ShareStatusType> value)
        {
            List<ViewModels.ShareStatusType> retVal = new List<ViewModels.ShareStatusType>();
            foreach (var type in value)
            {
                retVal.Add(type.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.FlagType ToViewModel(this DomainModel.FlagType value)
        {
            if (value == null)
                return null;

            return new ViewModels.FlagType
            {
                FlagTypeId = value.FlagTypeId,
                Name = value.Name
            };
        }

        public static ViewModels.Flag ToViewModel(this DomainModel.Flag value)
        {
            if (value == null)
                return null;

            return new ViewModels.Flag
            {
                FlagId = value.FlagId,
                FlagTypeId = value.FlagTypeId,
                AdoptedMapId = value.AdoptedMapId,
                Xpos = value.Xpos,
                Ypos = value.Ypos,
                Name = value.Name,
                Description = value.Description,
                VideoUrl = value.VideoUrl,
                PhotosUrl =  value.PhotosUrl,
                Date = value.Date,
                DateCreated = value.DateCreated,
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy,
                PartnerLogoId = value.PartnerLogoId,
                PartnerLogo = value.PartnerLogo.ToViewModel(),
                Address1 = value.Address1,
                Address2 = value.Address2,
                City = value.City,
                State = value.State,
                PostalCode = value.PostalCode

            };
        }

        public static ICollection<ViewModels.Flag> ToViewModel(this ICollection<DomainModel.Flag> value)
        {
            List<ViewModels.Flag> retVal = new List<ViewModels.Flag>();
            foreach (var flag in value)
            {
                retVal.Add(flag.ToViewModel());
            }
            return retVal;
        }       

        public static ViewModels.OrientationType ToViewModel(this DomainModel.OrientationType value)
        {
            if (value == null)
                return null;

            return new ViewModels.OrientationType
            {
                OrientationTypeId = value.OrientationTypeId,
                Name = value.Name
            };

        }

        public static ViewModels.PartnerLogo ToViewModel(this DomainModel.PartnerLogo value)
        {
            if (value == null)
                return null;

            return new ViewModels.PartnerLogo
            {
                PartnerLogoId = value.PartnerLogoId,
                Name = value.Name,
                IsActive = value.IsActive,
                ImageUrl = value.ImageUrl,
                Width = value.Width,
                Height = value.Height
            };

        }

        public static List<ViewModels.PartnerLogo> ToViewModel(this ICollection<DomainModel.PartnerLogo> value)
        {
            var retVal = new List<ViewModels.PartnerLogo>();
            foreach (var type in value)
            {
                retVal.Add(type.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.AspectRatio ToViewModel(this DomainModel.AspectRatio value)
        {
            if (value == null)
                return null;

            return new ViewModels.AspectRatio
            {
                AspectRatioId = value.AspectRatioId,
                Name = value.Name
            };

        }

        public static List<ViewModels.AspectRatio> ToViewModel(this ICollection<DomainModel.AspectRatio> value)
        {
            List<ViewModels.AspectRatio> retVal = new List<ViewModels.AspectRatio>();
            foreach (var type in value)
            {
                retVal.Add(type.ToViewModel());
            }
            return retVal;
        }

        public static List<ViewModels.Payment> ToViewModel(this ICollection<DomainModel.Payment> value)
        {
            List<ViewModels.Payment> retVal = new List<ViewModels.Payment>();
            foreach (var type in value)
            {
                retVal.Add(type.ToViewModel());
            }
            return retVal;
        }

        public static ViewModels.Payment ToViewModel(this DomainModel.Payment value)
        {
            if (value == null)
                return null;

            return new ViewModels.Payment
            {
                TransactionId = value.TransactionId,
                TransactionDate = value.TransactionDate,
                TransactionStatus = value.TransactionStatus,
                GrossTotal = value.GrossTotal,
                Currency = value.Currency,
                PayerFirstName = value.PayerFirstName,
                PayerLastName = value.PayerLastName,
                PayerEmail = value.PayerEmail,
                Tokens = value.Tokens,
            };

        }


    }
}