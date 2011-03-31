﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4952
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("photogalleryModel", "FK_Photos_Albums", "Album", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(Photogallery.DataAccessLayer.Concrete.Album), "Photo", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(Photogallery.DataAccessLayer.Concrete.Photo))]

// Original file name:
// Generation date: 24.03.2011 23:45:12
namespace Photogallery.DataAccessLayer.Concrete
{
    
    /// <summary>
    /// There are no comments for EntityContainer in the schema.
    /// </summary>
    public partial class EntityContainer : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new EntityContainer object using the connection string found in the 'EntityContainer' section of the application configuration file.
        /// </summary>
        public EntityContainer() : 
                base("name=EntityContainer", "EntityContainer")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new EntityContainer object.
        /// </summary>
        public EntityContainer(string connectionString) : 
                base(connectionString, "EntityContainer")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new EntityContainer object.
        /// </summary>
        public EntityContainer(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "EntityContainer")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for PhotoSet in the schema.
        /// </summary>
        public global::System.Data.Objects.ObjectQuery<Photo> PhotoSet
        {
            get
            {
                if ((this._PhotoSet == null))
                {
                    this._PhotoSet = base.CreateQuery<Photo>("[PhotoSet]");
                }
                return this._PhotoSet;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Photo> _PhotoSet;
        /// <summary>
        /// There are no comments for AlbumSet in the schema.
        /// </summary>
        public global::System.Data.Objects.ObjectQuery<Album> AlbumSet
        {
            get
            {
                if ((this._AlbumSet == null))
                {
                    this._AlbumSet = base.CreateQuery<Album>("[AlbumSet]");
                }
                return this._AlbumSet;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Album> _AlbumSet;
        /// <summary>
        /// There are no comments for PhotoSet in the schema.
        /// </summary>
        public void AddToPhotoSet(Photo photo)
        {
            base.AddObject("PhotoSet", photo);
        }
        /// <summary>
        /// There are no comments for AlbumSet in the schema.
        /// </summary>
        public void AddToAlbumSet(Album album)
        {
            base.AddObject("AlbumSet", album);
        }
    }
    /// <summary>
    /// There are no comments for photogalleryModel.Photo in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PhotoId
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="photogalleryModel", Name="Photo")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Photo : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Photo object.
        /// </summary>
        /// <param name="photoId">Initial value of PhotoId.</param>
        /// <param name="title">Initial value of Title.</param>
        /// <param name="description">Initial value of Description.</param>
        public static Photo CreatePhoto(global::System.Guid photoId, string title, string description)
        {
            Photo photo = new Photo();
            photo.PhotoId = photoId;
            photo.Title = title;
            photo.Description = description;
            return photo;
        }
        /// <summary>
        /// There are no comments for Property PhotoId in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Guid PhotoId
        {
            get
            {
                return this._PhotoId;
            }
            set
            {
                this.OnPhotoIdChanging(value);
                this.ReportPropertyChanging("PhotoId");
                this._PhotoId = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("PhotoId");
                this.OnPhotoIdChanged();
            }
        }
        private global::System.Guid _PhotoId;
        partial void OnPhotoIdChanging(global::System.Guid value);
        partial void OnPhotoIdChanged();
        /// <summary>
        /// There are no comments for Property Title in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this.OnTitleChanging(value);
                this.ReportPropertyChanging("Title");
                this._Title = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Title");
                this.OnTitleChanged();
            }
        }
        private string _Title;
        partial void OnTitleChanging(string value);
        partial void OnTitleChanged();
        /// <summary>
        /// There are no comments for Property Description in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this.ReportPropertyChanging("Description");
                this._Description = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Description");
                this.OnDescriptionChanged();
            }
        }
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// There are no comments for Album in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute("photogalleryModel", "FK_Photos_Albums", "Album")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public Album Album
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Album>("photogalleryModel.FK_Photos_Albums", "Album").Value;
            }
            set
            {
                ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Album>("photogalleryModel.FK_Photos_Albums", "Album").Value = value;
            }
        }
        /// <summary>
        /// There are no comments for Album in the schema.
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Data.Objects.DataClasses.EntityReference<Album> AlbumReference
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Album>("photogalleryModel.FK_Photos_Albums", "Album");
            }
            set
            {
                if ((value != null))
                {
                    ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<Album>("photogalleryModel.FK_Photos_Albums", "Album", value);
                }
            }
        }
    }
    /// <summary>
    /// There are no comments for photogalleryModel.Album in the schema.
    /// </summary>
    /// <KeyProperties>
    /// AlbumId
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="photogalleryModel", Name="Album")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Album : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Album object.
        /// </summary>
        /// <param name="albumId">Initial value of AlbumId.</param>
        /// <param name="description">Initial value of Description.</param>
        /// <param name="title">Initial value of Title.</param>
        public static Album CreateAlbum(global::System.Guid albumId, string description, string title)
        {
            Album album = new Album();
            album.AlbumId = albumId;
            album.Description = description;
            album.Title = title;
            return album;
        }
        /// <summary>
        /// There are no comments for Property AlbumId in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Guid AlbumId
        {
            get
            {
                return this._AlbumId;
            }
            set
            {
                this.OnAlbumIdChanging(value);
                this.ReportPropertyChanging("AlbumId");
                this._AlbumId = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("AlbumId");
                this.OnAlbumIdChanged();
            }
        }
        private global::System.Guid _AlbumId;
        partial void OnAlbumIdChanging(global::System.Guid value);
        partial void OnAlbumIdChanged();
        /// <summary>
        /// There are no comments for Property Description in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this.ReportPropertyChanging("Description");
                this._Description = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Description");
                this.OnDescriptionChanged();
            }
        }
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// There are no comments for Property Title in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this.OnTitleChanging(value);
                this.ReportPropertyChanging("Title");
                this._Title = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Title");
                this.OnTitleChanged();
            }
        }
        private string _Title;
        partial void OnTitleChanging(string value);
        partial void OnTitleChanged();
        /// <summary>
        /// There are no comments for Photos in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute("photogalleryModel", "FK_Photos_Albums", "Photo")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Data.Objects.DataClasses.EntityCollection<Photo> Photos
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Photo>("photogalleryModel.FK_Photos_Albums", "Photo");
            }
            set
            {
                if ((value != null))
                {
                    ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Photo>("photogalleryModel.FK_Photos_Albums", "Photo", value);
                }
            }
        }
    }
}
