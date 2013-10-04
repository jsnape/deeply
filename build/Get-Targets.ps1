Select-Xml -XPath //b:Target `
    -Path (Join-Path (Split-Path $MyInvocation.MyCommand.path -Parent) "build.proj") `
    -Namespace @{ b = 'http://schemas.microsoft.com/developer/msbuild/2003' } |
    Select-Object -ExpandProperty Node |
    ? {$_.Name -notlike "_*"} |
    Format-Table -Property Name -AutoSize