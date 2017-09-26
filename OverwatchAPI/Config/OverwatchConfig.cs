﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OverwatchAPI.Config
{
    public sealed class OverwatchConfig
    {
        public IReadOnlyList<Region> Regions { get; internal set; }

        public IReadOnlyList<Platform> Platforms { get; internal set; }

        internal OverwatchConfig() { }

        public sealed class Builder
        {
            private List<Region> _regions = new List<Region>();
            private List<Platform> _platforms = new List<Platform>();

            public Builder() { }

            /// <summary>
            /// Set the regions to use when auto-detecting. Order is preserved and will dictate the order that regions are detected in.
            /// Auto-detect will return the first succesful result.
            /// </summary>
            /// <param name="regions">The regions to detect.</param>
            /// <returns></returns>
            public Builder WithRegions(params Region[] regions)
            {
                this._regions = regions.Distinct().ToList();
                return this;
            }

            /// <summary>
            /// Sets the region detection to use the following order - US, EU, KR
            /// </summary>
            /// <returns></returns>
            public Builder WithAllRegions()
            {
                _regions = Enum.GetValues(typeof(Region)).Cast<Region>().ToList();
                _regions.Remove(Region.None);
                return this;
            }
            
            /// <summary>
            /// Sets the platforms to use when auto-detecting. Order is preserved and will dictate the order that the platforms are detected in.
            /// Console profiles do not have a set region, as such the first succesful result will be returned if a player has a profile on multiple systems.
            /// </summary>
            /// <param name="platforms"></param>
            /// <returns></returns>
            public Builder WithPlatforms(params Platform[] platforms)
            {
                this._platforms = platforms.Distinct().ToList();
                return this;
            }

            /// <summary>
            /// Sets the platform detection to use the following platforms in this order - PC, XBL, PSN
            /// </summary>
            /// <returns></returns>
            public Builder WithAllPlatforms()
            {
                _platforms = Enum.GetValues(typeof(Platform)).Cast<Platform>().ToList();
                return this;
            }

            public static implicit operator OverwatchConfig(Builder bldr)
            {
                return bldr.Build();
            }

            public OverwatchConfig Build()
            {
                return new OverwatchConfig()
                {
                    Regions = _regions,
                    Platforms = _platforms
                };
            }

            /// <summary>
            /// Default configuration - Includes all regions and all platforms.
            /// </summary>
            /// <returns></returns>
            public OverwatchConfig Default()
            {
                return WithAllPlatforms()
                    .WithAllRegions();
            }
        }
    }
}
