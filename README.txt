What is GenomeRunner?
******************
GenomeRunner is a program for annotation and enrichment analyses of user-provided genomic features of interest (FOIs) against (epi)genome annotation features (GFs) available from UCSC genome browser and stored in a MySQL database accessible for all GenomeRunner users. Currently, GenomeRunner works with NCBI36/hg18 and GRCh37/hg19 human genome assemblies, NCBI36/mm8 and NCBI/mm9 mouse genome assemblies, accepts FOIs from a tab-delimited .bed file format, and runs annotation/enrichment analysis against >6,000 GFs (genes, alternative splicing sites, transcription factor binding sites etc.) Annotation analysis provides a used with detailed annotation of each FOI against corresponding GFs, e.g. a set of SNPs may be annotated for co-localization and closest transctiption factor binding sites. Enrichment analysis calculates if a set of FOIs associated with genomic features more often that could happen by random chance.

Questions/bugs reporting/features request
******************
Mikhail G. Dozmorov <mikhail dot dozmorov at gmail>

What's new
****************** 11/25/2012
v. 3.1.0.0 GUI
+ Adding ability to load any GF as spot background, generate weighted random regions, hide unused/unfinished menu items, small fixes in menu items. Now, if one wants to use all genes to be used as a spot background, one should add gene table into "Genomic features that will be run" windos and select "File/Load selected Genomic Feature as spot background". Another way to do the same thing is to export gene table into a .bed file, and load it via "File/Open background file - Spot"
+ Use any SNP database as spot background for random sampling. This allows analysis of a set of SNPs against random set sampled from all SNPs in the table, not from the whole genome. Implementation is not very straightforward, as it requires manual addition of autoincrementing numerical "id" column to a SNP table. Therefore, access to this feature is currently placed in "Tools/Enrichment Analysis with SNP table as Background". Note that selecting this feature will prompt for selection of SNP table and then directly process with enrichment analysis using current settings.
+ Output fraction of FOIs overlapped with a GF, as matrix _PersentObsTot.gr. This is non-statistical overview what fraction out of total number of FOIs overlaps with a GF. If 100 - all FOIs overlap with a GF, if 0 - none of the FOIs overlap with GF.
! Minor fixes: Menu items adjusted, annotation output for 'Run Enrichment for all names' fixed, better handling of database connection string, binomial distribution formulae re-checking and minor adjustments, log file format adjustment.

****************** 01/30/2012
v. 3.0.0.0 GUI
+ Using chromInfo table for genome-specific background
+ Better Tier organisation for each organism
+ Ability to output FOI-specific enrichment matrixes, or combined matrixes
+ Annotation analysis - if NoOverlap is encountered, output either closest single GF, or closest left/right GF
+ Database dumps download available at http://wren.omrf.org/GenomeRunner/GenomeRunner.aspx
! Small fixes to improve speed and output layout of the results
v. 3.0.0.0 Command line
+ Using Settings from .xml file, this file copied into the folder with the results output
+ As console version processes one FOI file at a time, ability added to merge separate matrix files into one
! Rewrote help section, invoked when GenomeRunnerConsole.exe is called without parameters

v. 2.0.0.7
+ Addition of human GRCh37/hg19 and mouse NCBI37/mm9 data tables, selection of proper genomic background for each database
+ Option for calculating p-values using traditional Monte-Carlo simulations
! Improved random chromosome selection
! More compact log file; transposed matrix of -log10 p-values (FOIs horizontal, GFs vertical)
! Progress bar label shows the number of current Monte-Carlo run
! MySQL database connection timeout is set to 10 min

Prerequisites
******************
Windows XP and up. .NET 4 framework (will be installed automatically, if missing). Working Internet connection.

How to install/uninstall:
******************
Install: Run setup.exe and follow the install wizard. Please, check if the Internet connection is available, as GenomeRunner has to successfully connect to the remote database before first start. 
Uninstall: Open Control Panel, Add/remove program, select GenomeRunner and choose "Remove"
	
How to use:
******************
	See the tutorial at http://wren.omrf.org/GenomeRunner/GenomeRunner_Supplemental.htm
	or Documents\GenomeRunner - Supplemental.pdf
	
Web sites:
******************
GenomeRunner official sites:
	http://www.genomerunner.org
	http://sourceforge.org/p/genomerunner
	http://wren.omrf.org/GenomeRunner/GenomeRunner.aspx
	
Authors:
******************
Mikhail G. Dozmorov <mikhail dot dozmorov at gmail>
Lukas R. Cara <lks underscore cara at yahoo>
Cory B. Giles <cory dot b dot giles at gmail>
Jonathan D. Wren <jdwren at gmail>



