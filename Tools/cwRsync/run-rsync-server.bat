docker run --name rsync-server --rm -d -p 8000:873  -p 9000:22 -e USERNAME=root -e PASSWORD=root -v C:\Users\User\.ssh\id_rsa.pub:/root/.ssh/authorized_keys apnar/rsync-server
	
pause