all:
	dmcs -d:MONO  -r:System.dll -r:System.Data.dll -r:lib/MySql.Data.Entity.dll -r:lib/MySql.Data.dll -r:obj/x86/Debug/GenomeRunner.dll -r:lib/alglibnet2.dll  src/GenomeRunner/ui/console/GenomeRunnerMonoWrapper.cs -out:genome-runner.exe
	mkbundle --static -o genome-runner genome-runner.exe lib/alglibnet2.dll obj/x86/Debug/GenomeRunner.dll lib/MySql.Data.dll lib/MySql.Data.Entity.dll lib/Microsoft.VisualBasic.dll
	rm genome-runner.exe a.out
