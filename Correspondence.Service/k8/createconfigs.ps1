kubectl delete secret appsettings-secret-correspondenceservice --namespace fac
 
kubectl delete configmap appsettings-correspondenceservice --namespace fac

kubectl create secret generic appsettings-secret-correspondenceservice --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-correspondenceservice --namespace fac --from-file=../appsettings.json