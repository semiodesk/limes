import os
import shutil

def setupdir(path):
    if(os.path.isfile(path)):
        raise Exception(f"Path is a file: {path}")

    if os.path.isdir(path):
        shutil.rmtree(path)

    os.mkdir(path)

    return path

def replace(files, oldval, newval):
    print(f"Replacing all occurances of {oldval} with {newval} in {files}")
    os.system(f"sed -i 's/{oldval}/{newval}/g' {files}")