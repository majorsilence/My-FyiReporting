properties([parameters([
	booleanParam(defaultValue: false, description: 'Выкладывать сборку на сервер files.qsolution.ru', name: 'Publish'),
])])
node {
   stage('Git') {
      checkout([
         $class: 'GitSCM',
         branches: scm.branches,
         doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations,
         extensions: scm.extensions + [submodule(disableSubmodules: false, recursiveSubmodules: true)],
         userRemoteConfigs: scm.userRemoteConfigs
      ])
   }
   stage('Nuget') {
      sh 'nuget restore MajorsilenceReporting.sln'
   }
   stage('Build') {
      sh 'rm -f WinInstall/RdlDesigner-*.exe'
      sh 'WinInstall/makeWinInstall.sh'
      archiveArtifacts artifacts: 'WinInstall/RdlDesigner-*.exe', onlyIfSuccessful: true
   }
   if (params.Publish) {
      stage('VirusTotal'){
         sh 'vt scan file WinInstall/RdlDesigner-*.exe > file_hash'
         waitUntil (){
            sleep(30) //VirusTotal позволяет выполнить не более 4-х запросов за минуту.
            sh 'cut file_hash -d" " -f2 | vt analysis - > analysis'
            return readFile('analysis').contains('status: "completed"')
         }
         sh 'cat analysis'
         script {
            def status = readFile(file: "analysis")
            if ( !(status.contains('harmless: 0') && status.contains('malicious: 0') && status.contains('suspicious: 0'))) {
               unstable('VirusTotal in not clean')
            }
         }
      }
      stage('Publish'){
         sh 'scp WinInstall/RdlDesigner-*.exe a218160_qso@a218160.ftp.mchost.ru:subdomains/files/httpdocs/RdlDesigner/'
      }
   }
}
