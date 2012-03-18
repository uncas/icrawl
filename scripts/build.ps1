$framework = '4.0'

properties {
    $version_major = 1
    $version_minor = 0
    $version_build = 0
    $year = (Get-Date).year

    $configuration = "Release"

    $base_dir = ".\.."
    $output_dir = "$base_dir\build"

    $solution_file = "$base_dir\Uncas.WebTester.sln"
    $nunit_folder = "$base_dir\packages\NUnit.2.5.10.11092\tools"
    $nunit_exe = "$nunit_folder\nunit-console.exe"
}

task default -depends Test

task Init {
    if (!(Test-Path $output_dir))
    {
        mkdir $output_dir
    }

     . .\psake_ext.ps1

    Generate-Assembly-Info `
        -file "$base_dir\src\VersionInfo.cs" `
        -company "Uncas" `
        -product "Uncas.WebTester" `
        -version "$version_major.$version_minor.$version_build" `
        -copyright "Copyright (c) $year, Ole Lynge Sørensen"
}

task Compile -depends Init {
    msbuild $solution_file /p:Configuration=$configuration
}

task Test -depends Compile {
    $test_project_name = "Uncas.WebTester.Tests.Unit"
    $test_result_file = "$output_dir\$test_project_name.TestResult.xml"
    & $nunit_exe "$base_dir\test\$test_project_name\bin\$configuration\$test_project_name.dll" /xml=$test_result_file
}