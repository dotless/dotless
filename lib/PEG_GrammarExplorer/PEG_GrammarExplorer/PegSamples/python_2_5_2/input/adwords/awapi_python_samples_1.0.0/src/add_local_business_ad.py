#!/usr/bin/python
#
# Copyright 2008, Google Inc. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

"""This code sample creates a new local business ad given an existing ad
group. To create an ad group, you can run add_ad_group.py."""

import base64
import SOAPpy


# Provide AdWords login information.
email = 'INSERT_LOGIN_EMAIL_HERE'
password = 'INSERT_PASSWORD_HERE'
client_email = 'INSERT_CLIENT_LOGIN_EMAIL_HERE'
useragent = 'INSERT_COMPANY_NAME: AdWords API Python Sample Code'
developer_token = 'INSERT_DEVELOPER_TOKEN_HERE'
application_token = 'INSERT_APPLICATION_TOKEN_HERE'

# Define SOAP headers.
headers = SOAPpy.Types.headerType()
headers.email = email
headers.password = password
headers.clientEmail = client_email
headers.useragent = useragent
headers.developerToken = developer_token
headers.applicationToken = application_token

# Set up service connection. To view XML request/response, change value of
# ad_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
ad_service = SOAPpy.SOAPProxy(namespace + '/AdService',
                              header=headers)
ad_service.config.debug = 0

# Find similar businesses in Local Business Center.
business_name = 'Domino\'s Pizza'
business_address = '89 Charlwood St, London, SW1V 4PB'
business_country_code = 'GB'
in_local_business_center = 0

# Get business from Local Business Center or find similar business.
if in_local_business_center == 1:
  businesses = ad_service.getMyBusinesses()
else:
  businesses = ad_service.findBusinesses(business_name,
                                         business_address,
                                         business_country_code)

# Convert to a list if we get back a single object.
if len(businesses) > 0 and not isinstance(businesses, list):
  businesses = [businesses]

# Get business key.
for business in businesses:
  name = business['name']
  address = business['address']
  country_code = business['countryCode']

  if (business_name.find(name) > -1 or business_address.find(address) > -1 or
      business_country_code.find(country_code) > -1):
    business_key = business['key']

# Create new local business ad structure.
if business_key != None:
  ad_group_id = 'INSERT_AD_GROUP_ID_HERE'
  business_image = open('INSERT_BUSINESS_IMAGE_PATH_HERE', 'r').read()
  custom_icon = open('INSERT_CUSTOM_ICON_PATH_HERE', 'r').read()
  local_business_ad = {
    'adGroupId': SOAPpy.Types.untypedType(ad_group_id),
    'adType': SOAPpy.Types.untypedType('LocalBusinessAd'),
    'businessImage':
      {'data': SOAPpy.Types.untypedType(base64.encodestring(business_image))},
    'businessKey': business_key,
    'countryCode': SOAPpy.Types.untypedType('GB'),
    'customIcon':
      {'data': SOAPpy.Types.untypedType(base64.encodestring(custom_icon))},
    'description1': 'Choose from our delicious range now',
    'description2': 'Pre-order or delivered to your door',
    'destinationUrl': 'http://www.dominos.co.uk/',
    'displayUrl': 'www.dominos.co.uk'
  }

  # Check new ad for policy violations before adding it.
  language_target = {'languages': ['en']}
  geo_target = {'countryTargets': {'countries': ['GB']}}
  errors = ad_service.checkAds([local_business_ad], language_target, geo_target)

  # Convert to a list if we get back a single object.
  if len(errors) > 0 and not isinstance(errors, list):
    errors = [errors]

  # Add local business ad if there are no policy violations.
  if len(errors) == 0:
    ads = ad_service.addAds([local_business_ad])

    # Convert to a list if we get back a single object.
    if len(ads) > 0 and not isinstance(ads, list):
      ads = [ads]

    # Display new local business ad.
    for ad in ads:
      print 'New local business ad with name "%s" and ' \
          'id "%s" was created.' % (ad['businessName'], ad['id'])
  else:
    print 'New local business ad was not created due to the following policy ' \
        'violations:'
    for error in errors:
      print '  Detail: %s\nisExemptable: %s' % \
        (error['detail'], error['isExemptable'])
      print
else:
  print 'New local business ad was not created because business key was not ' \
      'found.'
