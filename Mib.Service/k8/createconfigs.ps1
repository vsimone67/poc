kubectl delete secret appsettings-secret-mibservice --namespace fac
 
kubectl delete configmap appsettings-mibservice --namespace fac

kubectl create secret generic appsettings-secret-mibservice --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-mibservice --namespace fac --from-file=../appsettings.json