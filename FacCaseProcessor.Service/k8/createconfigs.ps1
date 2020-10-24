kubectl delete secret appsettings-secret-caseservice --namespace fac
 
kubectl delete configmap appsettings-caseservice --namespace fac

kubectl create secret generic appsettings-secret-caseservice --namespace fac --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-caseservice --namespace fac --from-file=../appsettings.json