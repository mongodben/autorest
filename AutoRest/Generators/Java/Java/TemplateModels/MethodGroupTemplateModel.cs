﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Java.TemplateModels;
using Microsoft.Rest.Generator.Utilities;
using System.Globalization;

namespace Microsoft.Rest.Generator.Java
{
    public class MethodGroupTemplateModel : ServiceClient
    {
        public MethodGroupTemplateModel(ServiceClient serviceClient, string methodGroupName)
        {
            this.LoadFrom(serviceClient);
            MethodTemplateModels = new List<MethodTemplateModel>();
            // MethodGroup name and type are always the same but can be 
            // changed in derived classes
            MethodGroupName = methodGroupName;
            MethodGroupType = methodGroupName.ToPascalCase();
            Methods.Where(m => m.Group == MethodGroupName)
                .ForEach(m => MethodTemplateModels.Add(new MethodTemplateModel(m, serviceClient)));
        }
        public List<MethodTemplateModel> MethodTemplateModels { get; private set; }

        public string MethodGroupName { get; set; }

        public string MethodGroupType { get; set; }

        public string MethodGroupServiceType
        {
            get
            {
                string ret = MethodGroupType;
                if (MethodGroupType.EndsWith("Operations", StringComparison.Ordinal))
                {
                    ret = MethodGroupType.Substring(0, MethodGroupType.Length - 10);
                }
                return JavaCodeNamer.GetServiceName(ret);
            }
        }

        private HashSet<string> TypeImports(IList<IType> types)
        {
            HashSet<string> imports = new HashSet<string>();
            if (types == null)
            {
                return imports;
            }

            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                var sequenceType = type as SequenceType;
                var dictionaryType = type as DictionaryType;
                var primaryType = type as PrimaryType;
                if (sequenceType != null)
                {
                    imports.Add("java.util.List");
                    types.Add(sequenceType.ElementType);
                }
                else if (dictionaryType != null)
                {
                    imports.Add("java.util.Map");
                    types.Add(dictionaryType.ValueType);
                }
                else if (type is CompositeType || type is EnumType)
                {
                    imports.Add(string.Join(
                        ".",
                        this.Namespace.ToLower(CultureInfo.InvariantCulture),
                        "models",
                        type.Name));
                }
                else if (primaryType != null)
                {
                    var importedFrom = JavaCodeNamer.ImportedFrom(primaryType);
                    if (importedFrom != null)
                    {
                        imports.Add(importedFrom);
                    }
                }
            }
            return imports;
        }

        public IEnumerable<string> ImplImports
        {
            get
            {
                var parameters = this.MethodTemplateModels
                    .SelectMany(m => m.ParameterTemplateModels);

                var types = parameters.Select(p => p.Type)
                    .Concat(this.MethodTemplateModels.SelectMany(mtm => mtm.Responses.Select(res => res.Value)))
                    .Concat(this.MethodTemplateModels.Select(mtm => mtm.DefaultResponse))
                    .Distinct()
                    .ToList();

                HashSet<string> classes = TypeImports(types);

                if (this.MethodTemplateModels.Any(m => !m.ParametersToValidate.IsNullOrEmpty()))
                {
                    classes.Add("com.microsoft.rest.Validator");
                }

                IEnumerable<ParameterTemplateModel> nonBodyParams = parameters.Where(p => p.Location != ParameterLocation.Body);

                foreach (var param in nonBodyParams)
                {
                    if (param.Type.Name == "LocalDate" ||
                        param.Type.Name == "DateTime" ||
                        param.Type is CompositeType ||
                        param.Type is SequenceType ||
                        param.Type is DictionaryType)
                    {
                        classes.Add("com.microsoft.rest.serializer.JacksonConverterBuilder");
                    }
                    if (param.Type is SequenceType)
                    {
                        classes.Add("com.microsoft.rest.serializer.CollectionFormat");
                    }
                    if (param.Type == PrimaryType.ByteArray)
                    {
                        classes.Add("org.apache.commons.codec.binary.Base64");
                    }
                }
                
                return classes.AsEnumerable();
            }
        }

        public IEnumerable<string> InterfaceImports
        {
            get
            {
                IList<IType> types = this.MethodTemplateModels
                    .SelectMany(mtm => mtm.Parameters.Select(p => p.Type))
                    .Concat(this.MethodTemplateModels.Select(mtm => mtm.ReturnType))
                    .Distinct()
                    .ToList();

                HashSet<string> classes = TypeImports(types);

                foreach (var method in this.MethodTemplateModels)
                {
                    if (method.HttpMethod == HttpMethod.Delete)
                    {
                        classes.Add("com.microsoft.rest.DELETE");
                    }
                    else
                    {
                        classes.Add("retrofit.http." + method.HttpMethod.ToString().ToUpper(CultureInfo.InvariantCulture));
                    }
                    foreach (var param in method.Parameters)
                    {
                        if (param.Location != ParameterLocation.None &&
                            param.Location != ParameterLocation.FormData)
                            classes.Add("retrofit.http." + param.Location.ToString());
                    }
                }
                return classes.AsEnumerable();
            }
        }
    }
}