kubectl delete secret appsettings-secret-apigateway --namespace fac
 
kubectl delete configmap appsettings-apigateway --namespace fac

kubectl create secret generic appsettings-secret-apigateway --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-apigateway --namespace fac --from-file=../appsettings.json