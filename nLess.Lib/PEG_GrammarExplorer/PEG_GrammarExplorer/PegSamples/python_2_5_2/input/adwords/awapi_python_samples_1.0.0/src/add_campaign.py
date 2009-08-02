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

"""This code sample creates a new campaign with ad scheduling."""

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
# campaign_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
campaign_service = SOAPpy.SOAPProxy(namespace + '/CampaignService',
                                    header=headers)
campaign_service.config.debug = 0

# Create new campaign structure with ad scheduling set to show ads on Monday,
# Wednesday, and Friday from 8:00am to 5:00pm. Each bid is multiplied by 1.0.
interval_template = """
  <intervals>
    <day>%s</day>
    <endHour>%s</endHour>
    <endMinute>%s</endMinute>
    <multiplier>%s</multiplier>
    <startHour>%s</startHour>
    <startMinute>%s</startMinute>
  </intervals>"""
schedule_template = """
  %s
  <status>%s</status>"""
days = ['Monday', 'Wednesday', 'Friday']
intervals = ''
for index in range(len(days)):
  intervals += interval_template % (days[index], '17', '0', '1.0', '8', '0')
schedule = SOAPpy.Types.untypedType(schedule_template % (intervals, 'Enabled'))

# Create new campaign structure.
campaign = {
  'name': 'Sample Campaign',
  'budgetAmount': SOAPpy.Types.untypedType('100000'),
  'budgetPeriod': SOAPpy.Types.untypedType('Daily'),
  'geoTargeting': {'countryTargets': {'countries': ['US']}},
  'languageTargeting': {'languages': ['en']},
  'schedule': schedule
}

# Add campaign.
campaign = campaign_service.addCampaign(campaign)

# Display new campaign.
print 'New campaign with name "%s"  and id "%s" was created.' % \
    (campaign['name'], campaign['id'])
