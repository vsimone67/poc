kubectl delete secret appsettings-secret-hubservice --namespace fac
 
kubectl delete configmap appsettings-hubservice --namespace fac

kubectl create secret generic appsettings-secret-hubservice --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-hubservice --namespace fac --from-file=../appsettings.json