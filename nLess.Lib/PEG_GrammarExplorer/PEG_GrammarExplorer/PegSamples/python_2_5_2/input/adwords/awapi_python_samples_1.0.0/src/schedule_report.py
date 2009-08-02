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

"""This code sample retrieves a keyword report for the AdWords account that
belongs to the customer issuing the request."""

import gzip
import sys
import time
import urllib
import SOAPpy
import StringIO


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
# report_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
report_service = SOAPpy.SOAPProxy(namespace + '/ReportService',
                                  header=headers)
report_service.config.debug = 0

# Create report job structure.
report_job = """
  <selectedReportType>%s</selectedReportType>
  <name>%s</name>
  <aggregationTypes>%s</aggregationTypes>
  <adWordsType>%s</adWordsType>
  <keywordType>%s</keywordType>
  <startDay>%s</startDay>
  <endDay>%s</endDay>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>
  <selectedColumns>%s</selectedColumns>""" % \
  ('Keyword', 'Sample Keyword Report', 'Summary', 'SearchOnly', 'Broad',
   '2008-01-01', '2008-01-31', 'Campaign', 'AdGroup', 'Keyword',
   'KeywordStatus', 'KeywordMinCPC', 'KeywordDestUrlDisplay', 'Impressions',
   'Clicks', 'CTR', 'AveragePosition')
report_job = SOAPpy.Types.untypedType(report_job)
report_job._setAttr('xmlns:impl', namespace.replace('sandbox', 'adwords'))
report_job._setAttr('xsi:type', 'impl:DefinedReportJob')

try:
  # Validate report.
  report_service.validateReportJob(report_job)

  # Schedule report.
  job_id = report_service.scheduleReportJob(report_job)
  job_id = SOAPpy.Types.untypedType(job_id)

  # Wait for report to finish.
  status = report_service.getReportJobStatus(job_id)
  while status != 'Completed' and status != 'Failed':
    print 'Report job status is "%s".' % status
    time.sleep(30)
    status = report_service.getReportJobStatus(job_id)

  if(status == 'Failed'):
    print 'Report job generation failed.'
    sys.exit();

  # Download report.
  report_url = report_service.getGzipReportDownloadUrl(job_id)
  print 'Report is available at "%s".' % (report_url)

  # Write report to local file.
  report_data = urllib.urlopen(report_url).read()
  report_data = gzip.GzipFile(fileobj=StringIO.StringIO(report_data)).read()
  file = 'keyword_report.xml'
  output = open(file, 'w')
  try:
    output.write(report_data)
  finally:
    output.close()
  print 'Report has been written to "%s".' % (file)
except SOAPpy.Error, msg:
  print 'Report job failed validation due to the following error: "%s".' % (msg)
