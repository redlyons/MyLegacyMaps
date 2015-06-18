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
                MapTypes = value.MapTypes.ToViewModel(),
                OrientationType = value.OrientationType.ToViewModel()
            };
        }

        public static List<ViewModels.Map> ToViewModel(this ICollection<DomainModel.Map> value)
        {
            if (value == null)
                return null;

            List<ViewModels.Map> retVal = new List<ViewModels.Map>();
            foreach (var map in value)
            {
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

        public static ICollection<ViewModels.MapType> ToViewModel(this ICollection<DomainModel.MapType> value)
        {
            List<ViewModels.MapType> retVal = new List<ViewModels.MapType>();
            foreach (var type in value)
            {
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
                Date = value.Date,
                DateCreated = value.DateCreated,
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy

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
    }
}