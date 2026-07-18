pipeline {
    agent any

    environment {
        REGISTRY   = "localhost:5001"
        IMAGE_NAME = "prices-service"
        TAG        = "${BUILD_NUMBER}"
    }

    stages {
        stage('Checkout Code') {
            steps {
                checkout scm
            }
        }

        stage('Continuous Integration (Tests)') {
            steps {
                script {
                    echo "Executing Unit Tests inside Docker..."
                    // The --target flag stops the Dockerfile execution at the 'test' stage.
                    // If 'dotnet test' fails inside the container, Jenkins throws an error here.
                    sh "docker build --target test -t ${IMAGE_NAME}-test-env ."
                }
            }
        }

        stage('Continuous Delivery (Build)') {
            steps {
                script {
                    echo "Tests Passed! Building the final production image..."
                    // Because Docker caches layers, this will instantly skip the restore/build steps 
                    // and jump straight to the publish and final runtime stages.
                    sh "docker build -t ${REGISTRY}/${IMAGE_NAME}:${TAG} ."
                    sh "docker tag ${REGISTRY}/${IMAGE_NAME}:${TAG} ${REGISTRY}/${IMAGE_NAME}:latest"
                }
            }
        }

        stage('Deploy to Private Registry') {
            steps {
                script {
                    echo "Uploading verified artifact to the registry..."
                    sh "docker push ${REGISTRY}/${IMAGE_NAME}:${TAG}"
                    sh "docker push ${REGISTRY}/${IMAGE_NAME}:latest"
                }
            }
        }

        stage('Clean Up') {
            steps {
                script {
                    echo "Sweeping up temporary images..."
                    sh "docker rmi ${REGISTRY}/${IMAGE_NAME}:${TAG}"
                    sh "docker rmi ${REGISTRY}/${IMAGE_NAME}:latest"
                    // The || true prevents the pipeline from failing if the test image was already removed
                    sh "docker rmi ${IMAGE_NAME}-test-env || true" 
                }
            }
        }
    }

    post {
        success {
            echo "✅ CI/CD Pipeline Complete: Code tested, built, and deployed!"
        }
        failure {
            echo "❌ Pipeline Failed: Check the test output or build logs above."
        }
    }
}