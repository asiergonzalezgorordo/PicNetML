REM ECHO OFF

REM DOWNLOAD WEKA FROM: http://www.cs.waikato.ac.nz/ml/weka/snapshots/developer-branch.zip
REM DOWNLOAD WEKA PACKAGES FROM: http://sourceforge.net/projects/weka/files/weka-packages/

cd D:\git\PicNetML
lib\ikvm-7.2.4630.5\bin\ikvmc -debug -target:library^
 -classloader:ikvm.runtime.ClassPathAssemblyClassLoader^
 lib\weka\weka.jar lib\weka\packages\*.jar -out:lib\weka.dll -version:3.7.11.0 
