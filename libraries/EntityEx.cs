﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Connector.DirectLine
{
    public partial class Entity
    {
        [JsonExtensionData(ReadData = true, WriteData = true)]
        public JObject Properties { get; set; }

        /// <summary>
        /// Retrieve internal payload.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Set internal payload.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void SetAs<T>(T obj)
        {
            var entity = JsonConvert.DeserializeObject<Entity>(JsonConvert.SerializeObject(obj));
            this.Type = entity.Type;
            this.Properties = entity.Properties;
        }
    }
}
