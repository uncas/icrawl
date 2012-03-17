$framework = '4.0'

task default -depends Compile

task Init {

        <attribute type="System.Reflection.AssemblyCompanyAttribute" value="Uncas" />
        <attribute type="System.Reflection.AssemblyProductAttribute" value="Uncas.WebTester" />
        <attribute type="System.Reflection.AssemblyCopyrightAttribute" value="Copyright (c) 2012, Ole Lynge Sørensen" />
        <attribute type="System.Reflection.AssemblyVersionAttribute" value="${version.string}" />
        <attribute type="System.Reflection.AssemblyInformationalVersionAttribute" value="${version.string} (${git.hash})" />
        <attribute type="System.Reflection.AssemblyFileVersionAttribute" value="${version.string}" />

    Generate-Assembly-Info `
        -file "$base_dir\Rhino.Mocks\Properties\AssemblyInfo.cs" `
        -title "Rhino Mocks $version"
        -description "Mocking Framework for .NET"
        -company "Hibernating Rhinos"
        -product "Rhino Mocks $version"
        -version $version
        -copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
}

task Compile -depends Init {
    msbuild .\Uncas.WebTester.sln /p:Configuration=Release
}