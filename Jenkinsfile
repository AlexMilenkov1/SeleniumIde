pipeline {
    agent any
    stages {
        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }
        stage('Build Project') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }
        stage('Run dotnet tests') {
            steps {
                sh 'dotnet test'
            }
        }
    }
}