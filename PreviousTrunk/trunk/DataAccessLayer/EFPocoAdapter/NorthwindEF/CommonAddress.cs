// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public struct CommonAddress
    {
        private string _address;
        private string _city;
        private string _region;
        private string _postalCode;
        private string _country;

        public CommonAddress(string address, string city, string region, string postalCode, string country)
        {
            _address = address;
            _city = city;
            _region = region;
            _postalCode = postalCode;
            _country = country;
        }

        public String Address
        {
            get { return _address; }
        }
        public String City
        {
            get { return _city; }
        }

        public String Region
        {
            get { return _region; }
        }

        public String PostalCode
        {
            get { return _postalCode; }
        }

        public String Country
        {
            get { return _country; }
        }
    }
}
