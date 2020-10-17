kubectl delete secret appsettings-secret-authservice --namespace fac
 
kubectl delete configmap appsettings-authservice --namespace fac

kubectl create secret generic appsettings-secret-authservice --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-authservice --namespace fac --from-file=../appsettings.json