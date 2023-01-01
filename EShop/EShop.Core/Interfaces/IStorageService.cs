using EShop.Core.Common;

namespace EShop.Core.Interfaces
{
	public enum StoreFileType
	{
		ProductImage,
		CategoryImage
	}

	public interface IStorageService
	{
		public const int MaxFileSizeInMB = 50;
		public List<string> SafeExtensions { get; init; }

		public static bool CheckFileSize(long fileSizeInByte)
			=> (fileSizeInByte.ByteToMB() <= MaxFileSizeInMB) ? true : false;

		string GenerateRandomFileNameWithExtension(string extension);
		bool StoreFile(byte[] data, string fileName, StoreFileType storeFileType);
		bool RemoveFile(string fileName, StoreFileType storeFileType);
		public bool CheckFileExtension(string fileName);
	}
}
