$framework = '4.0'

properties {
    $base_dir = ".\.."
    $solutionFile = "$base_dir\Uncas.WebTester.sln"
    $version_major = 1
    $version_minor = 0
    $version_build = 0
}

task default -depends Compile

task Init {
     . .\psake_ext.ps1

    Generate-Assembly-Info `
        -file "$base_dir\src\VersionInfo.cs" `
        -company "Uncas" `
        -product "Uncas.WebTester" `
        -version "$version_major.$version_minor.$version_build" `
        -copyright "Copyright (c) 2012, Ole Lynge Sørensen"
}

task Compile -depends Init {
#    msbuild $solutionFile /p:Configuration=Release
}