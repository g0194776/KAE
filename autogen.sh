#imports the certificates what the Nuget needs.
sudo mozroots --import --machine --sync
sudo certmgr -ssl -m https://go.microsoft.com
sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo certmgr -ssl -m https://nuget.org
#Use the Nuget to restores packages what the project depended.
mono .nuget/NuGet.exe restore KJFramework-Family.sln
#start building.
xbuild /p:Configuration=Debug KJFramework-Family.sln
