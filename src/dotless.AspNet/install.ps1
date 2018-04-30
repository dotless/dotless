param($installPath, $toolsPath, $package, $project)

$xdtPath = ([IO.Path]::Combine($toolsPath, "web.config.install.xdt"));
$webConfigPath = ([IO.Path]::Combine([IO.Path]::GetDirectoryName($project.FullName), "web.config"));

function applyConfigTransformation($config,$xdt)
{
   Add-Type -Path ([io.path]::Combine($toolsPath, "Microsoft.Web.XmlTransform.dll"));
   try 
   {
      Write-Host 'Applying transformation on ' + $config
      $doc = New-Object Microsoft.Web.XmlTransform.XmlTransformableDocument
      $doc.PreserveWhiteSpace = $true
      $doc.Load($config)
      $trn = New-Object Microsoft.Web.XmlTransform.XmlTransformation($xdt)
      if ($trn.Apply($doc))
	  {
         $doc.Save($config)
      } else {
		throw "Transformation terminated"
	  }
   }
   catch
   {
      Write-Host $Error[0].Exception
   } 
}

applyConfigTransformation $webConfigPath $xdtPath