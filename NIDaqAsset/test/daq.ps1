Add-Type -Path "C:\Users\moha156x\OneDrive - Intel Corporation\Desktop\New folder\NIDaqAsset\NIDaqAsset\bin\Debug\Newtonsoft.Json.dll"
Add-Type -Path "C:\Users\moha156x\OneDrive - Intel Corporation\Desktop\New folder\NIDaqAsset\NIDaqAsset\bin\Debug\DaqDevice.dll"
[Reflection.Assembly]::LoadFrom("$path\DaqDevice.dll" )| Out-Null
$path ="C:\Users\moha156x\OneDrive - Intel Corporation\Desktop\New folder\NIDaqAsset\NIDaqAsset\test"
$DAQs = $null
$output = $null
$output = & "$path\NIDaqAsset.exe"
Write-Host $output
#$DAQs = [System.Management.Automation.PSSerializer]::Deserialize($output)
#$DAQs = [System.Web.Script.Serialization.JavaScriptSerializer]::new().Serialize($output)
#$DAQs = $output | ConvertFrom-Json 
#$xmlDoc = New-Object System.Xml.XmlDocument
#$xmlDoc.LoadXml($output)
#$xmlReader = New-Object System.Xml.XmlNodeReader($xmlDoc)
#$DAQs = $serializer.Deserialize($xmlReader);
add-type -assembly system.web.extensions
$ps_js = new-object system.web.script.serialization.javascriptSerializer

#The comma operator is the array construction operator in PowerShell
$DAQs=$ps_js.DeserializeObject($output)
foreach ($daq in $DAQs){
Write-Host $daq.model
}

