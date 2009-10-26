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

"""This code sample retrieves variations for a seed keyword."""

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
# keyword_tool_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
keyword_tool_service = SOAPpy.SOAPProxy(namespace + '/KeywordToolService',
                                        header=headers)
keyword_tool_service.config.debug = 0

# Create seed keyword structure.
seed_keyword = {
  'negative': SOAPpy.Types.untypedType('false'),
  'text': 'mars cruise',
  'type': SOAPpy.Types.untypedType('Broad')
}
use_synonyms = SOAPpy.Types.untypedType('true')

# Get keyword variations.
variation_lists = keyword_tool_service.getKeywordVariations([seed_keyword],
                                                            use_synonyms,
                                                            ['en'],
                                                            ['US'])

# Display keyword variations.
try:
  to_consider = variation_lists['additionalToConsider']
except AttributeError, msg:
  to_consider = []
print 'List of additional keywords to consider has %s variation(s).' % \
  (len(to_consider))

try:
  more_specific = variation_lists['moreSpecific']
except AttributeError, msg:
  more_specific = []
print 'List of popular queries with given seed has %s variation(s).' % \
  (len(more_specific))
