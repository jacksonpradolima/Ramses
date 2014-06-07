using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using System.Reflection;

namespace Ramses.Utils
{
    public class GlobalizationMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public GlobalizationMetadataProvider(bool requireConventionAttribute)
            : this(requireConventionAttribute, null)
        {
        }

        public GlobalizationMetadataProvider(bool requireConventionAttribute, Type defaultResourceType)
        {
            RequireConventionAttribute = requireConventionAttribute;
            DefaultResourceType = defaultResourceType;
        }

        // Whether or not the conventions only apply to classes with the MetadatawonventionsAttribute attribute applied.
        public bool RequireConventionAttribute
        {
            get;
            private set;
        }

        // Whether or not the conventions only apply to classes with the MetadataConventionsAttribute attribute applied.
        public Type DefaultResourceType
        {
            get;
            private set;
        }

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            Func<IEnumerable<Attribute>, ModelMetadata> metadataFactory = (attr) => base.CreateMetadata(attr, containerType, modelAccessor, modelType, propertyName);

            Type defaultResourceType = DefaultResourceType;

            ApplyConventionsToValidationAttributes(attributes, containerType, propertyName, defaultResourceType);
            ApplyDisplayAttributes(attributes, containerType, propertyName, defaultResourceType);

            return metadataFactory(attributes);

        }

        private static void ApplyDisplayAttributes(IEnumerable<Attribute> attributes,
                                                                    Type containerType, string propertyName,
                                                                    Type defaultResourceType)
        {
            var displayAttr = attributes.FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute)) as DisplayAttribute;

            if (displayAttr != null && !string.IsNullOrEmpty(displayAttr.Name))
                displayAttr.ResourceType = displayAttr.ResourceType ?? defaultResourceType;
        }


        private static void ApplyConventionsToValidationAttributes(IEnumerable<Attribute> attributes,
                                                                    Type containerType, string propertyName,
                                                                    Type defaultResourceType)
        {
            foreach (ValidationAttribute validationAttribute in attributes.Where(a => (a as ValidationAttribute != null)))
            {
                validationAttribute.ErrorMessageResourceType = validationAttribute.ErrorMessageResourceType ?? defaultResourceType;
            }
        }

    }

}