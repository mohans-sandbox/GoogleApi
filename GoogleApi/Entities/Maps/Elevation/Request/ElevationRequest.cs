﻿using System;
using System.Collections.Generic;
using System.Linq;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Interfaces;
using GoogleApi.Entities.Common.Extensions;

namespace GoogleApi.Entities.Maps.Elevation.Request
{
    /// <summary>
    /// Elevation Request.
    /// </summary>
    public class ElevationRequest : BaseMapsChannelRequest, IRequestQueryString
    {
        /// <inheritdoc />
        protected internal override string BaseUrl => base.BaseUrl + "elevation/json";

        /// <summary>
        /// Sampled path requests are indicated through use of the path and samples parameters, indicating a request for elevation data along a path at specified intervals. 
        /// As with positional requests using the locations parameter, the path parameter specifies a set of latitude and longitude values. Unlike a positional request, however, 
        /// the path specifies an ordered set of vertices. Rather than return elevation data only at the vertices, path requests are sampled along the length of the path, 
        /// based on the number of samples specified (inclusive of the endpoints).
        /// The path parameter may take either of the following arguments:
        /// An array of two or more comma-separated coordinate text strings separated using the pipe ('|') character: path=40.714728,-73.998672|-34.397,150.644
        /// Encoded coordinates using the Encoded Polyline Algorithm: path=enc:gfo}EtohhUxD@bAxJmGF
        /// Latitude and longitude coordinate strings are defined using numerals within a comma-separated text string. For example, "40.714728,-73.998672|-34.397, 150.644" is a valid path value. Latitude and longitude values must correspond to a valid location on the face of the earth. Latitudes can take any value between -90 and 90 while longitude values can take any value between -180 and 180. If you specify an invalid latitude or longitude value, your request will be rejected as a bad request.
        /// You may pass any number of multiple coordinates within an array or encoded polyline, as long as you don't exceed the service quotas, while still constructing a valid URL. Note that when passing multiple coordinates, the accuracy of any returned data may be of lower resolution than when requesting data for a single coordinate
        /// </summary>
        public virtual IEnumerable<Coordinate> Path { get; set; } = new List<Coordinate>();

        /// <summary>
        /// Locations defines the location(s) on the earth from which to return elevation data. 
        /// This parameter takes either a single location as a comma-separated {latitude,longitude} pair (e.g. "40.714728,-73.998672") or multiple latitude/longitude pairs 
        /// passed as an array or as an encoded polyline. For more information.
        /// Note: Either this or Path property must be set, this overrides the Path.
        /// </summary>
        public virtual IEnumerable<Coordinate> Locations { get; set; } = new List<Coordinate>();

        /// <summary>
        /// Required when using the Path, and specifies the number of sample points along a path for which to return elevation data. 
        /// The samples parameter divides the given path into an ordered set of equidistant points along the path.
        /// </summary>
        public virtual int? Samples { get; set; }

        /// <inheritdoc />
        public override IList<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            var parameters = base.GetQueryStringParameters();

            if ((this.Locations == null || !this.Locations.Any()) && (this.Path == null || !this.Path.Any()))
                throw new ArgumentException($"'{nameof(this.Locations)}' or '{nameof(this.Path)}' is required");

            if (this.Locations != null && this.Locations.Any() && this.Path != null && this.Path.Any())
                throw new ArgumentException($"'{nameof(this.Path)}' and '{nameof(this.Locations)}' cannot both be specified");

            if (this.Path != null && this.Path.Any())
            {
                if (this.Path.Count() <= 1)
                    throw new ArgumentException($"A minimum of two coordinates is required when using '{nameof(this.Path)}'");

                if (this.Samples == null)
                    throw new ArgumentException($"'{nameof(this.Samples)}' is required, when using '{nameof(this.Path)}'");

                parameters.Add("path", string.Join("|", this.Path));
                parameters.Add("samples", this.Samples.ToString());
            }

            if (this.Locations != null && this.Locations.Any())
            {
                parameters.Add("locations", string.Join("|", this.Locations));
            }

            return parameters;
        }
    }
}