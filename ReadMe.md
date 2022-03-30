# Environment-Certificates-Lab

Web-application that shows the certificates on its host.

![.github/workflows/Docker-deploy.yml](https://github.com/HansKindberg-Lab/Environment-Certificates-Lab/actions/workflows/Docker-deploy.yml/badge.svg)

Web-application, without configuration, pushed to Docker Hub: https://hub.docker.com/r/hanskindberg/environment-certificates-lab

## Authentication

Since we show certificates, it is best that we require authentication. You need to enter a secret to get authorized. The secret is empty by default. You configure the secret in appsettings or in the environment-variables.

In appsettings.json

	{
		"Authentication": {
			"Secret": "your-preferred-secret"
		}
	}