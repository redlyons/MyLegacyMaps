using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = MLM.Models;
using ViewModels = MyLegacyMaps.Models;

namespace MyLegacyMaps.Extensions
{
    public static class DomainModelConverter
    {        
        public static DomainModel.AdoptedMap ToDomainModel(this ViewModels.AdoptedMap value)
        {
            if (value == null)
                return null;

            return new DomainModel.AdoptedMap
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
                    Map = value.Map.ToDomainModel(),
                    ShareStatusType = value.ShareStatusType.ToDomainModel(),
                    Flags = value.Flags.ToDomainModel()
                    
                };
        }
        
        public static DomainModel.Map ToDomainModel(this ViewModels.Map value)
        {
            if (value == null)
                return null;

            return new DomainModel.Map
            {
                MapId = value.MapId,
                Name = value.Name,
                Description = value.Description,
                FileName = value.FileName,
                ImageUrl = value.ImageUrl,
                ThumbUrl = value.ThumbUrl,
                OrientationTypeId = value.OrientationTypeId,
                IsActive = value.IsActive,
                DateCreated = value.DateCreated,   
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy,                
                MapTypes = value.MapTypes.ToDomainModel(),
                OrientationType = value.OrientationType.ToDomainModel()
            };
        }

        public static DomainModel.MapType ToDomainModel(this ViewModels.MapType value)
        {
            if (value == null)
                return null;

            return new DomainModel.MapType
                {
                    MapTypeId = value.MapTypeId,
                    Name = value.Name,
                    IsActive = value.IsActive
                };
        }

        public static ICollection<DomainModel.MapType> ToDomainModel(this ICollection<ViewModels.MapType> value)
        {
            if (value == null)
                return null;

            List<DomainModel.MapType> retVal = new List<DomainModel.MapType>();
            foreach (var type in value)
            {
                retVal.Add(type.ToDomainModel());
            }
            return retVal;
        }

        public static DomainModel.ShareStatusType ToDomainModel( this ViewModels.ShareStatusType value)
        {
            if (value == null)
                return null;

            return new DomainModel.ShareStatusType
                {
                    ShareStatusTypeId = value.ShareStatusTypeId,
                    Name = value.Name
                };
        }

        public static DomainModel.FlagType ToDomainModel( this ViewModels.FlagType value)
        {
            if (value == null)
                return null;

            return new DomainModel.FlagType
            {
                FlagTypeId = value.FlagTypeId,
                Name = value.Name
            };
        }

        public static DomainModel.Flag ToDomainModel(this ViewModels.Flag value)
        {
            if (value == null)
                return null;

            return new DomainModel.Flag
            {
                FlagId = value.FlagId,
                FlagTypeId = value.FlagTypeId,
                AdoptedMapId = value.AdoptedMapId,
                Xpos = value.Xpos,
                Ypos = value.Ypos,
                Name = value.Name,
                Description = value.Description,
                VideoUrl = value.VideoUrl,
                PhotosUrl = value.PhotosUrl,
                Date = value.Date,
                DateCreated = value.DateCreated,
                DateModified = value.DateModified,
                ModifiedBy = value.ModifiedBy


            };
        }
               
        public static ICollection<DomainModel.Flag> ToDomainModel( this ICollection<ViewModels.Flag> value)
        {
            if (value == null)
                return null;

            List<DomainModel.Flag> retVal = new List<DomainModel.Flag>();
            foreach(var flag in value)
            {
                retVal.Add(flag.ToDomainModel());
            }
            return retVal;
        }       

        public static DomainModel.OrientationType ToDomainModel(this ViewModels.OrientationType value)
        {
            if (value == null)
                return null;

            return new DomainModel.OrientationType
            {
                OrientationTypeId = value.OrientationTypeId,
                Name = value.Name
            };

        }

       
    }
}