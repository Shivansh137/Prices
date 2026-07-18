pipeline {
    agent any

    environment {
        // Points to the registry container running on the shared Docker network
        REGISTRY   = "local-registry:5000"
        IMAGE_NAME = "prices-service"
        TAG        = "${BUILD_NUMBER}"
    }

    stages {
        stage('Sanity Check') {
            steps {
                echo "Verifying environment capabilities..."
                // Verify Jenkins can talk to the host's Docker engine
                sh 'docker version'
            }
        }

        stage('Checkout Code') {
            steps {
                // Pulls the latest commits from your GitHub repository automatically
                checkout scm
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    echo "Starting build for image: ${IMAGE_NAME}:${TAG}"
                    
                    // Triggers the multi-stage Dockerfile build step
                    sh "docker build -t ${REGISTRY}/${IMAGE_NAME}:${TAG} ."
                    
                    // Also tag it as 'latest' for local development ease
                    sh "docker tag ${REGISTRY}/${IMAGE_NAME}:${TAG} ${REGISTRY}/${IMAGE_NAME}:latest"
                }
            }
        }

        stage('Push to Private Registry') {
            steps {
                script {
                    echo "Pushing built images to our local registry..."
                    
                    // Push the unique build number tag
                    sh "docker push ${REGISTRY}/${IMAGE_NAME}:${TAG}"
                    
                    // Push the latest tag
                    sh "docker push ${REGISTRY}/${IMAGE_NAME}:latest"
                }
            }
        }

        stage('Clean Up Workspace') {
            steps {
                echo "Cleaning up local images from host to save space..."
                // Removes local references so your laptop disk space isn't consumed
                sh "docker rmi ${REGISTRY}/${IMAGE_NAME}:${TAG}"
                sh "docker rmi ${REGISTRY}/${IMAGE_NAME}:latest"
            }
        }
    }

    post {
        success {
            echo "Successfully built and pushed ${IMAGE_NAME} to the local architecture ecosystem!"
        }
        failure {
            echo "Pipeline failed. Check the compilation step or Docker daemon connectivity above."
        }
    }
}