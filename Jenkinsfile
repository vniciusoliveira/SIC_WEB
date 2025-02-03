pipeline {
    agent { label 'dotnet_agent' }
    when {
        branch 'main'
    }
    environment {
        PROJECT_PATH = 'API_SIC_WEB_ANGULAR'
        DB_CONNECTION = credentials('db-connection')
        EPONTO_CONNECTION = credentials('eponto-connection')
    }
    
    stages {
        stage('Build no Docker') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
                sh 'ls -la'
                sh 'dotnet publish --configuration Release --runtime win-x64 --self-contained false -o ./publish'
            }
        }

        
        stage('Configure Application') {
            steps {
                script {
                    def appSettings = """
{
  "ConnectionStrings": {
    "DataBase": "${DB_CONNECTION}",
    "EPONTO": "${EPONTO_CONNECTION}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
"""
                    writeFile file: 'publish/appsettings.json', text: appSettings
                }
            }
        }
        stage('Stop IIS Pool') {
            when {
                expression { env.BRANCH_NAME == 'main' }
            }
            steps {
                script {
                    def poolName = 'API_SIC_TESTE'
                    def siteName = 'TMKT-ZL-WA06'
                    def sitePath = '/API_SIC_TESTE'
                    def physicalPath = 'E:\\WEBSITES\\TMKT_TESTE\\API_SIC'
                    def appcmd = 'C:\\Windows\\System32\\inetsrv\\appcmd.exe'
                    
                    sshPublisher(
                        publishers: [
                            sshPublisherDesc(
                                configName: 'TMKT-ZL-WA06',
                                transfers: [
                                    sshTransfer(
                                        sourceFiles: '',
                                        remoteDirectory: '',
                                        execCommand: """
                                            REM Verifica se o pool existe
                                            ${appcmd} list apppool ${poolName}
                                            if %errorlevel% neq 0 (
                                                echo Pool nao existe, criando...
                                                ${appcmd} add apppool /name:${poolName}
                                                ${appcmd} set apppool /apppool.name:${poolName} /managedRuntimeVersion:v4.0
                                                ${appcmd} set apppool /apppool.name:${poolName} /managedPipelineMode:Integrated
                                                ${appcmd} set apppool /apppool.name:${poolName} /startMode:AlwaysRunning
                                                
                                                REM Verifica se o site existe
                                                ${appcmd} list app ${siteName}${sitePath}
                                                if %errorlevel% neq 0 (
                                                    echo Site nao existe, criando...
                                                    ${appcmd} add app /site.name:"${siteName}" /path:"${sitePath}" /physicalPath:"${physicalPath}" /applicationPool:"${poolName}"
                                                )
                                            )
                                            
                                            REM Para o pool em qualquer caso
                                            ${appcmd} stop apppool /apppool.name:${poolName}
                                            
                                            REM Aguarda 5 segundos
                                            timeout /t 5
                                        """
                                    )
                                ],
                                usePromotionTimestamp: false,
                                useWorkspaceInPromotion: false,
                                verbose: true
                            )
                        ]
                    )
                }
            }
        }
        stage('Deploy via Publish Over SSH') {
            when {
                expression { env.BRANCH_NAME == 'main' }
            }
            steps {
                sh 'ls -la publish/'
                sshPublisher(
                    publishers: [
                        sshPublisherDesc(
                            configName: 'TMKT-ZL-WA06',
                            transfers: [
                                sshTransfer(
                                    sourceFiles: 'publish/**/*',
                                    remoteDirectory: 'TMKT_TESTE/API_SIC',
                                    removePrefix: 'publish',
                                    cleanRemote: false,
                                    flatten: false,
                                    excludes: '',
                                    patternSeparator: '[, ]+',
                                    noDefaultExcludes: true,
                                    execCommand: '''
                                        if not exist "E:\\Websites\\TMKT_TESTE\\API_SIC" mkdir "E:\\Websites\\TMKT_TESTE\\API_SIC"
                                    '''
                                )
                            ],
                            usePromotionTimestamp: false,
                            useWorkspaceInPromotion: false,
                            verbose: true
                        )
                    ]
                )
            }
        }
        stage('Start IIS Pool') {
            when {
                expression { env.BRANCH_NAME == 'main' }
            }
            steps {
                script {
                    def poolName = 'API_SIC_TESTE'
                    def appcmd = 'C:\\Windows\\System32\\inetsrv\\appcmd.exe'
                    
                    sshPublisher(
                        publishers: [
                            sshPublisherDesc(
                                configName: 'TMKT-ZL-WA06',
                                transfers: [
                                    sshTransfer(
                                        sourceFiles: '',
                                        remoteDirectory: '',
                                        execCommand: "${appcmd} start apppool /apppool.name:${poolName}"
                                    )
                                ],
                                usePromotionTimestamp: false,
                                useWorkspaceInPromotion: false,
                                verbose: true
                            )
                        ]
                    )
                }
            }
        }
    }
    
    post {
        success {
            echo 'Deploy realizado com sucesso!'
        }
        failure {
            echo 'Falha no deploy!'
        }
        always {
            // Limpa arquivos temporários
            sh 'rm -f publish.tar.gz'
        }
    }
}