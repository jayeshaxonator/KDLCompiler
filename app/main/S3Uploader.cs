using Amazon.S3;
using Amazon.S3.Transfer;
using System.Collections.Generic;


namespace KDLCompiler
{

    public class S3Uploader
    {
        private string bucketName;
        private string folderPrefix;
        private AmazonS3Client s3Client;

        public S3Uploader(string accessKey, string secretKey, string region, string bucketName, string folderPrefix)
        {
            this.bucketName = bucketName;
            this.folderPrefix = folderPrefix;

            this.s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(region));
        }

        public void UploadFiles(string[] files)
        {
            foreach (string file in files)
            {
                // Determine the S3 key for the file based on the prefix and file name
                string s3Key = folderPrefix + "/" + System.IO.Path.GetFileName(file);

                // Create a transfer utility to upload the file to S3
                TransferUtility transferUtility = new TransferUtility(s3Client);
                transferUtility.Upload(file, bucketName, s3Key);
            }
        }
    }
}
