# My Dynamic Route53 DNS Service

An ASP.NET Web API service to dynamically update your Route53 record. Uses the DynDNS protocol, so you can get this to work with a DynDNS compatible router.

I wanted to use my personal domain, didn't want to pay for a 3rd party DDNS service, and wanted to do some coding, so here it is. It follows the no-ip.com API, which is DynDNS compatible.

Instructions (ie: `mydomain.com`):
1) Set up your Route53 A record in AWS first, ie: `home.mydomain.com`
2) Create a `Web.Release.config` file and put your secrets in
```xml
<?xml version="1.0" encoding="utf-8"?>

<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
    <appSettings>
        <add key="UserName"
             value="XXXXX"
             xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
        <add key="Password"
             value="XXXXX"
             xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
        <add key="HostedZoneId"
             value="XXXXX"
             xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
        <add key="AwsAccessKeyId"
             value="XXXXX"
             xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
        <add key="AwsSecretAccessKey"
             value="XXXXX"
             xdt:Transform="SetAttributes" xdt:Locator="Match(key)" />
    </appSettings>
    <system.web>
        <compilation xdt:Transform="RemoveAttributes(debug)" />
    </system.web>
    <system.webServer>
        <rewrite>
            <rules>
                <clear />
                <rule name="Redirect to https" stopProcessing="true" xdt:Transform="Insert">
                    <match url="(.*)" />
                    <conditions>
                        <add input="{HTTPS}" pattern="off" ignoreCase="true" />
                    </conditions>
                    <action type="Redirect" url="https://{HTTP_HOST}{REQUEST_URI}" redirectType="Permanent"
                            appendQueryString="false" />
                </rule>
            </rules>
            <rewriteMaps />
        </rewrite>
    </system.webServer>
</configuration>
```
3) Set up your hosting, ie: `dynupdate.mydomain.com`
4) Set up Let's Encrypt (I use https://github.com/Lone-Coder/letsencrypt-win-simple) to get a free SSL certificate for `dynupdate.mydomain.com` (Optional, remove rewrite rule if you don't want to use SSL)
5) Test the endpoint: `https://dynupdate.mydomain.com/nic/update?hostname=home.mydomain.com&myip=1.2.3.4` with Basic Authentication (UserName/password in Web.config)
6) Use DynDNS option on your router (I use a Ubiquiti UniFi Security Gateway) to set up Dynamic DNS for `home.mydomain.com`


